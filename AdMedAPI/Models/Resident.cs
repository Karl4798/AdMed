﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdMedAPI.Models
{
    public class Resident
    {

        // General information of the resident
        [Key] public int Id { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        public enum Genders { Female, Male, Other }
        [Required] public Application.Genders Gender { get; set; }
        public string GenderString { get; set; }
        [Required] public string Allergies { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        [Required] public string IdentityNumber { get; set; }
        [Required] public string MedicalAid { get; set; }
        public string MedicalAidNumber { get; set; }
        public string DoctorName { get; set; }
        public string HomeTelephoneNumber { get; set; }
        public string WorkTelephoneNumber { get; set; }
        [Required] public string CellTelephoneNumber { get; set; }
        [Required] public string Undertaker { get; set; }
        [Required] public string UndertakerTelephoneNumber { get; set; }
        [Required] public string PharmacyName { get; set; }
        [Required] public string PharmacyTelephoneNumber { get; set; }
        [Required] public string PharmacyFaxNumber { get; set; }
        [Required] public int PrimaryContactId { get; set; }
        [ForeignKey("PrimaryContactId")] public virtual PrimaryContact PrimaryContact { get; set; }
        [Required] public int MedicationId { get; set; }
        [ForeignKey("MedicationId")] public virtual Medication Medication { get; set; }

        // General information of the primary contact included in PrimaryContact

    }
}
