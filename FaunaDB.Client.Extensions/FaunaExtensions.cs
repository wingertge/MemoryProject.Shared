﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FaunaDB.Query;
using FaunaDB.Types;

namespace FaunaDB.Extensions
{
    public static class FaunaExtensions
    {
        public static Expr ToFaunaObj(this object obj)
        {
            var fields = new Dictionary<string, Expr>();
            foreach (var prop in obj.GetType().GetProperties())
            {
                var propType = prop.PropertyType;
                if(propType.Name.StartsWith("CompositeIndex")) continue;
                var propValue = prop.GetValue(obj);
                var nameAttr = prop.GetCustomAttribute(typeof(FaunaFieldAttribute)) as FaunaFieldAttribute;
                var propName = nameAttr?.Name?.Replace("data.", "") ?? prop.Name;
                if (propName == "@ref" || propName == "ts") continue;
                switch (Type.GetTypeCode(propType))
                {
                    case TypeCode.Byte:
                    case TypeCode.SByte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Single:
                    case TypeCode.String:
                    case TypeCode.Boolean:
                        fields[propName] = (dynamic) propValue;
                        continue;
                    case TypeCode.Object:
                        var referenceAttr = prop.GetCustomAttribute(typeof(ReferenceAttribute));

                        if (typeof(IEnumerable).IsAssignableFrom(propType))
                        {
                            fields[propName] = referenceAttr == null 
                                ? Language.Arr(((IEnumerable)propValue).Cast<object>().Select(a => a.ToFaunaObjOrPrimitive()).ToArray()) 
                                : Language.Arr(((IEnumerable)propValue).Cast<IReferenceType>().Select(a => Language.Ref(a.Id)).ToArray());
                            continue;
                        }

                        fields[propName] = referenceAttr == null ? propValue.ToFaunaObj() : Language.Ref(((IReferenceType)propValue).Id);
                        continue;
                    case TypeCode.Char:
                        fields[propName] = propValue.ToString();
                        continue;
                    case TypeCode.DateTime:
                        fields[propName] = ((DateTime)propValue).ToUnixTimeStamp();
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return Language.Obj(fields);
        }

        internal static Expr ToFaunaObjOrPrimitive(this object obj)
        {
            switch (Type.GetTypeCode(obj.GetType()))
            {
                case TypeCode.Object:
                    return obj.ToFaunaObj();
                case TypeCode.DBNull:
                    return Language.Null();
                case TypeCode.Boolean:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.String:
                    return (dynamic) obj;
                case TypeCode.Char:
                    return obj.ToString();
                case TypeCode.DateTime:
                    return ((DateTime)obj).ToUnixTimeStamp(); ;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static bool IsPrimitive(this object obj)
        {
            var type = obj.GetType();
            return type.IsPrimitive || type == typeof(decimal) || type == typeof(string);
        }
    }
}