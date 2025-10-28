using Microsoft.AspNetCore.Mvc;

namespace CalculatorApp.Controllers
{
    public class CalculatorController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    
        [HttpPost]
        public IActionResult Calculate(double? left, double? right, string? op)
        {
            
            if (left == null || right == null || string.IsNullOrWhiteSpace(op))
            {
                ViewBag.Error = "Por favor introduce ambos números y selecciona una operación.";
                return View("Index");
            }

            double l = left.Value;
            double r = right.Value;

            double result = 0;
            string? error = null;

            try
            {
                switch (op)
                {
                    case "add":
                        result = l + r;
                        break;
                    case "subtract":
                        result = l - r;
                        break;
                    case "multiply":
                        result = l * r;
                        break;
                    case "divide":
                        if (r == 0)
                        {
                            error = "División por cero no permitida.";
                        }
                        else
                        {
                            result = l / r;
                        }
                        break;
                    default:
                        error = "Operación no reconocida.";
                        break;
                }
            }
            catch (System.Exception ex)
            {
                error = "Error: " + ex.Message;
            }

            if (error != null)
            {
                ViewBag.Error = error;
                return View("Index");
            }

            ViewBag.Result = result;
            ViewBag.Left = l;
            ViewBag.Right = r;
            ViewBag.Op = op;
            return View("Index");
        }
    }
}
