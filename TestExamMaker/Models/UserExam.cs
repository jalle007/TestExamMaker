//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestExamMaker.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserExam
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public Nullable<int> ExamID { get; set; }
        public Nullable<double> Score { get; set; }
        public Nullable<bool> Passed { get; set; }
    
        public virtual Exam Exam { get; set; }
    }
}
