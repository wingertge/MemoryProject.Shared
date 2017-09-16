﻿/* tslint:disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v11.7.2.0 (NJsonSchema v9.6.3.0) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming

namespace MemoryApi.HttpClient {

export interface IAuthClient {
    /**
     * attemps a log in with post body
     * @body Login form data
     * @return successful login, token returned
     */
    login(body: LoginModel): Promise<string>;
    /**
     * attempts a log in with provider details in post body
     * @body Provider login data
     * @return successful login, token returned
     */
    loginProvider(body: LoginProviderModel): Promise<string>;
    /**
     * valiates a user token
     * @x_AuthToken Auth token to validate
     * @return auth token validated successfully
     */
    validate(x_AuthToken: string): Promise<string>;
}

export class AuthClient implements IAuthClient {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        this.http = http ? http : <any>window;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * attemps a log in with post body
     * @body Login form data
     * @return successful login, token returned
     */
    login(body: LoginModel): Promise<string> {
        let url_ = this.baseUrl + "/auth/login";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_ = <RequestInit>{
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json", 
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processLogin(_response);
        });
    }

    protected processLogin(response: Response): Promise<string> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v, k) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 !== undefined ? resultData200 : <any>null;
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = resultData400 !== undefined ? resultData400 : <any>null;
            return throwException("A server error occurred.", status, _responseText, _headers, result400);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<string>(<any>null);
    }

    /**
     * attempts a log in with provider details in post body
     * @body Provider login data
     * @return successful login, token returned
     */
    loginProvider(body: LoginProviderModel): Promise<string> {
        let url_ = this.baseUrl + "/auth/login-provider";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_ = <RequestInit>{
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json", 
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processLoginProvider(_response);
        });
    }

    protected processLoginProvider(response: Response): Promise<string> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v, k) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 !== undefined ? resultData200 : <any>null;
            return result200;
            });
        } else if (status === 401) {
            return response.text().then((_responseText) => {
            return throwException("A server error occurred.", status, _responseText, _headers);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<string>(<any>null);
    }

    /**
     * valiates a user token
     * @x_AuthToken Auth token to validate
     * @return auth token validated successfully
     */
    validate(x_AuthToken: string): Promise<string> {
        let url_ = this.baseUrl + "/auth/validate";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <RequestInit>{
            method: "POST",
            headers: {
                "X-AuthToken": x_AuthToken, 
                "Content-Type": "application/json", 
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processValidate(_response);
        });
    }

    protected processValidate(response: Response): Promise<string> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v, k) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 !== undefined ? resultData200 : <any>null;
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = resultData400 !== undefined ? resultData400 : <any>null;
            return throwException("A server error occurred.", status, _responseText, _headers, result400);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<string>(<any>null);
    }
}

export interface IUsersClient {
    /**
     * registers a user with the service
     * @body Registration form data
     * @return successful registration and automatic login
     */
    register(body: RegisterModel): Promise<string>;
}

export class UsersClient implements IUsersClient {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        this.http = http ? http : <any>window;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * registers a user with the service
     * @body Registration form data
     * @return successful registration and automatic login
     */
    register(body: RegisterModel): Promise<string> {
        let url_ = this.baseUrl + "/users/register";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_ = <RequestInit>{
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json", 
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processRegister(_response);
        });
    }

    protected processRegister(response: Response): Promise<string> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v, k) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 !== undefined ? resultData200 : <any>null;
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = resultData400 !== undefined ? resultData400 : <any>null;
            return throwException("A server error occurred.", status, _responseText, _headers, result400);
            });
        } else if (status === 500) {
            return response.text().then((_responseText) => {
            return throwException("A server error occurred.", status, _responseText, _headers);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<string>(<any>null);
    }
}

export class LoginModel implements ILoginModel {
    identifier?: string | undefined;
    password?: string | undefined;

    constructor(data?: ILoginModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.identifier = data["identifier"];
            this.password = data["password"];
        }
    }

    static fromJS(data: any): LoginModel {
        let result = new LoginModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["identifier"] = this.identifier;
        data["password"] = this.password;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new LoginModel();
        result.init(json);
        return result;
    }
}

export interface ILoginModel {
    identifier?: string | undefined;
    password?: string | undefined;
}

export class LoginProviderModel implements ILoginProviderModel {
    provider?: string | undefined;
    accessToken?: string | undefined;

    constructor(data?: ILoginProviderModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.provider = data["provider"];
            this.accessToken = data["accessToken"];
        }
    }

    static fromJS(data: any): LoginProviderModel {
        let result = new LoginProviderModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["provider"] = this.provider;
        data["accessToken"] = this.accessToken;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new LoginProviderModel();
        result.init(json);
        return result;
    }
}

export interface ILoginProviderModel {
    provider?: string | undefined;
    accessToken?: string | undefined;
}

export class RegisterModel implements IRegisterModel {
    /** email address for new user */
    email?: string | undefined;
    /** username for new user login */
    username?: string | undefined;
    /** password for new account */
    password?: string | undefined;
    /** password validation */
    passwordAgain?: string | undefined;

    constructor(data?: IRegisterModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.email = data["email"];
            this.username = data["username"];
            this.password = data["password"];
            this.passwordAgain = data["passwordAgain"];
        }
    }

    static fromJS(data: any): RegisterModel {
        let result = new RegisterModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["email"] = this.email;
        data["username"] = this.username;
        data["password"] = this.password;
        data["passwordAgain"] = this.passwordAgain;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new RegisterModel();
        result.init(json);
        return result;
    }
}

export interface IRegisterModel {
    /** email address for new user */
    email?: string | undefined;
    /** username for new user login */
    username?: string | undefined;
    /** password for new account */
    password?: string | undefined;
    /** password validation */
    passwordAgain?: string | undefined;
}

export class SwaggerException extends Error {
    message: string;
    status: number; 
    response: string; 
	headers: { [key: string]: any; };
    result: any; 

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
		super();

        this.message = message;
        this.status = status;
        this.response = response;
		this.headers = headers;
        this.result = result;
    }

    protected isSwaggerException = true;

    static isSwaggerException(obj: any): obj is SwaggerException {
        return obj.isSwaggerException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): any {
    throw new SwaggerException(message, status, response, headers, result);
}

}