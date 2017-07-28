using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestExamMaker.Models {
  public class AnswerViewModel
    {
        public  Question Question { get; set; }
        public  List<QuestionAnswer> Answers { get; set; }
    }
  }