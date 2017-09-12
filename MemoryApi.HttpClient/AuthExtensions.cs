// Code generated by Microsoft (R) AutoRest Code Generator 1.2.2.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace MemoryApi.HttpClient
{
    using MemoryCore.JsonModels;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for Auth.
    /// </summary>
    public static partial class AuthExtensions
    {
            /// <summary>
            /// attemps a log in with post body
            /// </summary>
            /// <remarks>
            /// Validates the user credentials specified in the post body and generates a
            /// login session if correct.
            ///
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='body'>
            /// Login form data
            /// </param>
            public static string Login(this IAuth operations, LoginModel body)
            {
                return operations.LoginAsync(body).GetAwaiter().GetResult();
            }

            /// <summary>
            /// attemps a log in with post body
            /// </summary>
            /// <remarks>
            /// Validates the user credentials specified in the post body and generates a
            /// login session if correct.
            ///
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='body'>
            /// Login form data
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<string> LoginAsync(this IAuth operations, LoginModel body, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.LoginWithHttpMessagesAsync(body, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// valiates a user token
            /// </summary>
            /// <remarks>
            /// Validates the authentication token in the header to establish access. Used
            /// internally and to validate token on client startup.
            ///
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='xAuthToken'>
            /// Auth token to validate
            /// </param>
            public static string Validate(this IAuth operations, string xAuthToken)
            {
                return operations.ValidateAsync(xAuthToken).GetAwaiter().GetResult();
            }

            /// <summary>
            /// valiates a user token
            /// </summary>
            /// <remarks>
            /// Validates the authentication token in the header to establish access. Used
            /// internally and to validate token on client startup.
            ///
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='xAuthToken'>
            /// Auth token to validate
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<string> ValidateAsync(this IAuth operations, string xAuthToken, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ValidateWithHttpMessagesAsync(xAuthToken, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
