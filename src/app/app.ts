import { Component, signal } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router'; 
import { CommonModule } from '@angular/common'; 

@Component({
  selector: 'app-root',
  standalone: true, 
  // C'est ICI qu'on insère les modules : dans la liste "imports"
  imports: [
    RouterOutlet, 
    RouterLink, 
    RouterLinkActive, 
    CommonModule
  ], 
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('LeaveApp-Front');
}