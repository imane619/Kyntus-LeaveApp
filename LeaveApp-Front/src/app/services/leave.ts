import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LeaveService {
  // L'URL de ton API .NET (port 5066 d'après tes captures Swagger)
  private apiUrl = 'http://localhost:5066/api/Leaves';

  constructor(private http: HttpClient) {}

  /**
   * Envoie les données du formulaire au Backend
   */
  submitLeave(leaveData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/submit`, leaveData);
  }
}