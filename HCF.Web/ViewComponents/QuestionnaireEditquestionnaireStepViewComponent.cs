using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BDO;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class QuestionnaireEditquestionnaireStepViewComponent :ViewComponent
    {
        public QuestionnaireEditquestionnaireStepViewComponent()
        {

        }
        public async Task<IViewComponentResult> InvokeAsync(int rowscount, QuestionnaireSteps questionnaireSteps = null)
        {
            var objquestionnaireSteps = new QuestionnaireSteps();
            if (questionnaireSteps != null)
                objquestionnaireSteps = questionnaireSteps;
            ViewData["i"] = rowscount;
            return await Task.FromResult(View(objquestionnaireSteps));
        }
    }
}
