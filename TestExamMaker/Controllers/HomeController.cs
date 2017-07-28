using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestExamMaker.Models;

namespace TestExamMaker.Controllers {

  [Authorize]
  public class HomeController : Controller {

    Entities db = new Entities();
    Question question;

    //start page
    public ActionResult Index () {
      var exam = db.Exam.FirstOrDefault();
      var qs = db.Question.Where(q => q.ExamID == exam.ID);
      examQuestions = new List<Question>(qs);

      ViewBag.Name = exam.Name;
      ViewBag.Description = exam.Description;
      ViewBag.Duration = exam.ExamDuration;
      
      return View();
      }
    
    //show question
    public ActionResult Question (int? order) {
      if (order == null) order = 1;

      if (reachedEnd(order))
        return RedirectToAction("Result");

      question = examQuestions.FirstOrDefault(q => q.Order == order);
      if (question == null) return new EmptyResult();

      ViewBag.Question = question;
      var answers = question.QuestionAnswer;

      return View(answers.ToList<QuestionAnswer>());
      }
    
    //post answer
    [HttpPost]
    public ActionResult Question (int[] Answers, int? radioGrp) {
      var order = int.Parse(Request.QueryString["order"]);
      var MultipleAnswers = bool.Parse(Request.Form["MultipleAnswers"]);
      var QID = int.Parse(Request.Form["QID"]);

      var curq = examQuestions.FirstOrDefault(q => q.ID == QID);

      if (radioGrp != null) {
        curq.QuestionAnswer.FirstOrDefault(i => i.DisplayOrder == radioGrp).Answer = true;
        } else if (MultipleAnswers && Answers != null)       //save multiple
           {
        foreach (var answer in Answers) {
          var ans = curq.QuestionAnswer.FirstOrDefault(q => q.DisplayOrder == answer);
          if (ans != null) ans.Answer = true;
          }
        }

      return RedirectToAction("Question", new { order = order });
      }
    
   //display results
    public ActionResult Result () {
      //calculate score and save to Score    
      decimal score =  CalculateScore(examQuestions);
      
      ViewBag.Score = score;
      ViewBag.Count = examQuestions.Count();

      var passingScore = db.Exam.FirstOrDefault().PassScore;
      ViewBag.Passed = ViewBag.Score >= passingScore;

      var userExam = new UserExam() { 
              Username=User.Identity.Name,
              ExamID=db.Exam.FirstOrDefault().ID,
              Score=(double)score,
              Passed=ViewBag.Passed
      };
      db.UserExam.Add(userExam);
      db.SaveChanges();

      return View();
      }

    //Helpers
    public decimal CalculateScore (List<Question> examQuestions) {
      decimal Score = 0;

      foreach (var question in examQuestions) {
        int temp = 0;

        if (question.MultipleAnswers) {
          var possibleAnswers = question.QuestionAnswer.Where(a => a.CorrectChoice == true).Count();

          foreach (var answ in question.QuestionAnswer) {
            if (answ.CorrectChoice == answ.Answer) temp += 1;
            }
          Score = Score + (decimal)temp / possibleAnswers;

          } else {
          foreach (var answ in question.QuestionAnswer) {
            if (answ.Answer == answ.CorrectChoice)
              Score += 1;
            }
          }

        }
      return Score;
      }

    private List<Question> examQuestions {
      get { return Session["questions"] as List<Question>; }
      set { Session["questions"] = value; }
      }
    public bool reachedEnd (int? order) {
      if (order == null) return false;

      return examQuestions.Count() == order - 1;
      }

    }
  }