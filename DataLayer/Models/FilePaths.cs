using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    internal static class FilePaths
    {
        //private static string ROOT = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin") - 1);

        private const string DOC_BASE = "\\Documents\\"; // root PSDF folder

        public const string DOCUMENTS_FILE_DIR = "\\Documents\\"; // root Documents folder

        public const string INSTRUCTOR_FILE_DIR = DOC_BASE + "Instructor\\";
        public const string TRAINEE_PROFILE_DIR = DOC_BASE + "Traniee_Profiles\\";
        public const string TRAINEE_DOCUMENTS = TRAINEE_PROFILE_DIR + "TraineeDocuments\\";

        public const string TRAINEE_PROFILE_RESULT_DOCUMENT_DIR = TRAINEE_PROFILE_DIR + "ResultDocument\\";
        public const string TRAINEE_PROFILE_CNIC_IMG_DIR = TRAINEE_PROFILE_DIR + "CNICImage\\";
        ///***************DDV-PATHS***********///
        public const string TRAINEE_PROFILE_NADRA_DIR = DOC_BASE + "Traniee_Profiles\\"+"NADRA\\";
        public const string NADRA_VERYSISFILES_DIR = @"C:\Nadra_VerysisFiles";
        public const string NADRA_TARGETFILES_DIR = @"C:\\NADRA";
        ///***************DDV-PATHS***********///
        public const string VisitPlan_FILE_DIR = DOC_BASE + "VisitPlan\\";
        public const string TSP_FILE_DIR = DOC_BASE + "TSPFiles\\";
        public const string EMPLOYMENT_DIR = DOC_BASE + "TSPEmployment\\";
        public const string PROGRAM_DESIGN_DOC = DOC_BASE + "ProgramDesign";


    }
}