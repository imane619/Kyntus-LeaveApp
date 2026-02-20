import { Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard';

export const routes: Routes = [
  // chemin vide, on affiche le Dashboard
  { path: '', component: DashboardComponent },
  
  // Quand on clique sur "Demander un congé"
  { path: 'demander', component: DashboardComponent },
  
  // une adresse qui n'existe pas,  revenir à l'accueil
  { path: '**', redirectTo: '' }
];
