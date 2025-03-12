

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace HEMUdaan.Models
{
  public class QuestionViewModel
  {
    public QuestionViewModel() => this.QuestionOption = new List<QuestionOptionViewModel>();

    [Key]
    public int? QuestionID { get; set; }

    [AllowHtml]
    public string QuestionName { get; set; }

    public int? Marks { get; set; }

    public string Type { get; set; }

    public string ComplexityLevel { get; set; }

    public int QuestionLevel { get; set; }

    public bool isAdminStatus { get; set; }

    public string AnswerDescription { get; set; }

    public int? CourseID { get; set; }

    public int? TopicID { get; set; }

    public int? SubTopicID { get; set; }

    public List<QuestionOptionViewModel> QuestionOption { get; set; }

    public int? languageID { get; set; }
  }
}
