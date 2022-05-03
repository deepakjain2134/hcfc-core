using System.Collections.Generic;
using System.Linq;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Web.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class QuestionnaireController : BaseController
    {
      
        private readonly IQuestionnairesService _questionnairesService;

        public QuestionnaireController(IQuestionnairesService questionnairesService)
        {           
            _questionnairesService = questionnairesService;
        }


        // [CrxAuthorize(Roles = "Questionnaire_Index")]
        public ActionResult Index()
        {
            UISession.AddCurrentPage("Plant_Ops", 0, "Plant Ops");
            var lists = _questionnairesService.GetQuestionnaires();
            return View(lists);
        }

        public ActionResult QuestionnaireSteps(int questionnaireId)
        {
            var questionnaire = _questionnairesService.GetQuestionnaire(questionnaireId);
            return View("_questionnaireSteps", questionnaire);
        }


        public ActionResult AddQuestionnaire(int? questionnaireId)
        {
            var questionnaire = new Questionnaire();
            if (questionnaireId.HasValue && questionnaireId.Value > 0)
            {
                questionnaire = _questionnairesService.GetQuestionnaire(questionnaireId.Value);
            }
            //else
            //{
            //    QuestionnaireSteps steps = new QuestionnaireSteps();
            //    steps.Step = string.Empty;
            //    //steps.RecommendedValue = string.Empty;
            //    steps.IsActive = true;
            //    questionnaire.QuestionnaireStep.QuestionnaireStepDetail = new List<QuestionnaireStepDetail>();

            //}
            return View(questionnaire);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestionnaire(Questionnaire questionnaire)
        {
            var userid = Base.UserSession.CurrentUser.UserId;
            questionnaire.CreatedBy = userid;
            var questionnaireId = 0;
            questionnaireId = _questionnairesService.AddQuestionnaire(questionnaire);
            return questionnaireId > 0 ? (ActionResult)RedirectToAction("Index") : View();
        }

        public ActionResult AddQuestionnaireSteps(int? questionnaireId, int? questionnaireStepId = 0)
        {
            var questionnaire = new Questionnaire();
            if (questionnaireId.HasValue && questionnaireId.Value > 0)
            {
                questionnaire = _questionnairesService.GetQuestionnaire(questionnaireId.Value);
                if (questionnaireStepId > 0)
                {
                    questionnaire.QuestionnaireStep = questionnaire.QuestionnaireSteps.FirstOrDefault(x => x.QuestionnaireStepId == questionnaireStepId);
                }
                if (questionnaire.QuestionnaireStep == null)
                {
                    questionnaire.QuestionnaireStep = new QuestionnaireSteps
                    {
                        QuestionnaireStepDetail = new List<QuestionnaireStepDetail>()
                    };
                    if (questionnaire.QuestionnaireOptions.Count > 0)
                    {
                        foreach (var item in questionnaire.QuestionnaireOptions)
                        {
                            var objQuestionnaireStepDetail = new QuestionnaireStepDetail();
                            questionnaire.QuestionnaireStep.QuestionnaireStepDetail.Add(objQuestionnaireStepDetail);
                        }
                    }
                    else
                    {
                        var objQuestionnaireStepDetail = new QuestionnaireStepDetail();
                        questionnaire.QuestionnaireStep.QuestionnaireStepDetail.Add(objQuestionnaireStepDetail);
                    }
                }
            }
            return PartialView("_AddQuestionnaireSteps", questionnaire);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestionnaireSteps(Questionnaire questionnaire)
        {
            if (questionnaire.QuestionnaireId > 0)
            {
                if (!string.IsNullOrEmpty(questionnaire.QuestionnaireStep.Step))
                {
                    questionnaire.QuestionnaireStep.QuestionnaireId = questionnaire.QuestionnaireId;
                    questionnaire.QuestionnaireStep.CreatedBy = Base.UserSession.CurrentUser.UserId;
                    var quesstepsId = _questionnairesService.AddQuestionnaireSteps(questionnaire.QuestionnaireStep);
                    foreach (var questionnairestepDetail in questionnaire.QuestionnaireStep.QuestionnaireStepDetail)
                    {
                        questionnairestepDetail.QuestionnaireStepId = quesstepsId;
                        questionnairestepDetail.QuestionnaireId = questionnaire.QuestionnaireId;
                        questionnairestepDetail.CreatedBy = Base.UserSession.CurrentUser.UserId;
                        _questionnairesService.AddQuestionnaireStepsDetails(questionnairestepDetail);
                    }
                }
            }
            return RedirectToAction("Index");
            //return View(questionnaire);
        }

        public ActionResult EditquestionnaireStep(int rowscount, QuestionnaireSteps questionnaireSteps = null)
        {

            var objquestionnaireSteps = new QuestionnaireSteps();
            if (questionnaireSteps != null)
                objquestionnaireSteps = questionnaireSteps;
            ViewData["i"] = rowscount;
            return PartialView("_editquestionnaireStep", objquestionnaireSteps);
        }



        private void AddQuestionnaireStep(Questionnaire questionnaire, int userid, int questionnaireId)
        {
            foreach (var questionnairestep in questionnaire.QuestionnaireSteps)
            {
                questionnairestep.CreatedBy = userid;
                questionnairestep.QuestionnaireId = questionnaireId;
                if (!string.IsNullOrEmpty(questionnairestep.Step))
                { _questionnairesService.AddQuestionnaireSteps(questionnairestep); }
            }
        }


        public void UpdateQuestionnaireSequence(string seqIds)
        {
            _questionnairesService.UpdateQuestionnaireSequence(seqIds);
        }

        public void UpdateQuestionnaireStepSequence(string seqIds, string quesstepId, string quesnnaireid)
        {
            _questionnairesService.UpdateQuestionnaireStepSequence(seqIds, quesstepId, quesnnaireid);
        }

        public void UpdateQuestionnaireSteps(int quesstepsId, bool status)
        {
            var objQuestionnaireSteps = new QuestionnaireSteps
            {
                QuestionnaireStepId = quesstepsId,
                IsActive = status
            };
            _questionnairesService.UpdateQuestionnaireSteps(objQuestionnaireSteps);
        }
    }
}