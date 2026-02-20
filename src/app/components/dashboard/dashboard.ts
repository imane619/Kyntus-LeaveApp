import { Component, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeaveRequestForm } from '../leave-request-form/leave-request-form';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    LeaveRequestForm
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class DashboardComponent implements OnInit {
  
  isDrawerOpen = false; 
  isLoading = signal(true);

  constructor() {}

  ngOnInit() {
    // Simulation du chargement microservice
    setTimeout(() => {
      this.isLoading.set(false);
    }, 1500);
  }

  toggleDrawer() {
    this.isDrawerOpen = !this.isDrawerOpen;
  }
}