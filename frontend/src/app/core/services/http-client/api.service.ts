import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { ApiResponse } from '../../interfaces/api-response';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private readonly baseUrl:string = environment.apiUrl;

  constructor(private http: HttpClient) {}

  public get<T>(endpoint: string): Observable<ApiResponse<T>> {
    return this.http.get<ApiResponse<T>>(`${this.baseUrl}/${endpoint}`);
  }

  public getAll<T>(endpoint: string): Observable<ApiResponse<T[]>> {
    return this.http.get<ApiResponse<T[]>>(`${this.baseUrl}/${endpoint}`);
  }

  public getById<T>(endpoint: string, id: string): Observable<ApiResponse<T>> {
    return this.http.get<ApiResponse<T>>(`${this.baseUrl}/${endpoint}?${id}`);
  }

  public add<T, B = unknown>(endpoint: string, body: B): Observable<ApiResponse<T>> {
    return this.http.post<ApiResponse<T>>(`${this.baseUrl}/${endpoint}`, body);
  }

    public updateOurParams<T, B = unknown>(endpoint: string, body: B): Observable<ApiResponse<T>> {
    return this.http.put<ApiResponse<T>>(`${this.baseUrl}/${endpoint}`, body);
  }

  public update<T, B = unknown>(endpoint: string, id: number, body: B): Observable<ApiResponse<T>> {
    return this.http.put<ApiResponse<T>>(`${this.baseUrl}/${endpoint}/${id}`, body);
  }

  public delete<T>(endpoint: string, id: string): Observable<ApiResponse<T>> {
    return this.http.delete<ApiResponse<T>>(`${this.baseUrl}/${endpoint}?${id}`);
  }


  public validatorSession(){
     return this.http.get(`${this.baseUrl}/User/info-session`, { withCredentials: true });
  }
 

}
