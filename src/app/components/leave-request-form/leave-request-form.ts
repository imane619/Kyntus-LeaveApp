import { Component } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { FormsModule } from '@angular/forms'; 
import { LeaveService } from '../../services/leave';

@Component({
  selector: 'app-leave-request-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './leave-request-form.html', 
  styleUrl: './leave-request-form.css',
})
export class LeaveRequestForm { 
  // --- Données du formulaire ---
  selectedReason: string = ''; 
  startDate: string = '';
  endDate: string = '';
  justification: string = '';
  leaveBalance: number = 15; 
  requestStatus: string = 'Nouvelle'; 

  // --- Variables pour la simulation d'impact ---
  impactPercentage: number = 100; 
  impactMessage: string = 'Analyse de couverture prête';
  impactStatus: string = 'Good';

  // --- CORRECTION : Injection du service indispensable ---
  constructor(private leaveService: LeaveService) {}

  // --- GESTION DES ÉVÉNEMENTS ---
  onReasonChange(event: any) {
    this.selectedReason = event.target.value;
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    console.log('Fichier sélectionné:', file);
  }

  // --- LOGIQUE MÉTIER ---

  calculateDays(): number {
    if (!this.startDate || !this.endDate) return 0;
    
    const start = new Date(this.startDate);
    const end = new Date(this.endDate);
    
    // Calcul de la différence en millisecondes
    const diff = end.getTime() - start.getTime();
    
    // Conversion en jours
    const days = Math.ceil(diff / (1000 * 60 * 60 * 24)) + 1;
    
    return days;
  }

  isSubmitDisabled(): boolean {
    const duration = this.calculateDays();

    // Bloquer si Date Fin < Date Début
    if (this.startDate && this.endDate && duration <= 0) return true;

    // Bloquer si Congé > Solde
    if (this.selectedReason === 'conge' && duration > this.leaveBalance) return true;

    // Bloquer si champs obligatoires vides
    return !this.selectedReason || !this.startDate || !this.endDate;
  }

  onDateChange() {
    if (this.startDate && this.endDate && this.calculateDays() > 0) {
      this.simulateImpact();
    } else {
      this.impactPercentage = 100;
      this.impactStatus = 'Good';
      this.impactMessage = 'Veuillez saisir des dates valides';
    }
  }

  simulateImpact() {
    this.impactPercentage = 75; 
    this.impactMessage = "Impact équipe : Modéré";
    this.impactStatus = "Warning";
  }

  // --- APPEL AU BACKEND ---
  onSubmit() {
    console.log("Tentative d'envoi...", this.selectedReason, this.startDate);
    if (!this.isSubmitDisabled()) {
      const data = {
        employeeId: "EMP-IMANE", 
        cellId: "CELL-ALPHA",
        // Utilisation de toISOString pour la compatibilité .NET DateTime
        startDate: new Date(this.startDate).toISOString(), 
        endDate: new Date(this.endDate).toISOString(),
        type: this.selectedReason,
        justification: this.justification,
        attachmentUrl: ""
      };

      this.leaveService.submitLeave(data).subscribe({
        next: (response) => {
          console.log("Succès Backend:", response);
          this.requestStatus = 'En attente';
          alert("Demande envoyée avec succès au serveur !");
        },
        error: (err) => {
          console.error("Erreur Backend:", err);
          alert("Erreur de connexion : Vérifie que ton API .NET tourne sur le port 5066");
        }
      });
    }
  }
}