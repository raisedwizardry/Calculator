using Calculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public interface IStateManager<Quo>
{
    void Save(string name, Quo state);
    Quo Load(string name);
}

public class SessionStateManager<Quo> : IStateManager<Quo>
{
    public void Save(string name, Quo state)
    {
        HttpContext.Current.Session[name] = state;
    }
    public Quo Load(string name)
    {
        return (Quo)HttpContext.Current.Session[name];
    }
}

namespace Calculator.Controllers
{
    public class CalculatorController : Controller
    {
        protected IStateManager<CalculatorModel> stateManager = new SessionStateManager<CalculatorModel>();
        public void SetStateManager(IStateManager<CalculatorModel> manager)
        {
            stateManager = manager;
        }

        public ActionResult Index()
        {
            CalculatorModel calculator = new CalculatorModel();
            stateManager.Save("model", calculator);

            return View(calculator);
        }

        [HttpPost]
        public ActionResult Index(string param, string operate)
        {
            CalculatorModel calculator = stateManager.Load("model");

            if (param != null)
                calculator.Process(param);
            else if (operate != null)
                calculator.DoOperate(operate);
            stateManager.Save("model", calculator);

            return View("Index", calculator);
        }
    }
}