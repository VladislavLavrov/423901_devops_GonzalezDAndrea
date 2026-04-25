using App_practical.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace App_practical.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View(null);

        [HttpPost]
        public IActionResult Index(string value1, string value2, string operation)
        {
            if (!double.TryParse(value1.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double v1) ||
                !double.TryParse(value2.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double v2))
            {
                return View(new DataViewModel { ErrorMessage = "Было введено неверное число." });
            }

            double result = operation switch
            {
                "Сложить (+)" => v1 + v2,
                "Вычесть (-)" => v1 - v2,
                "Умножить (*)" => v1 * v2,
                "Разделить (/)" => v2 != 0 ? v1 / v2 : 0,
                "Возвести в степень (^)" => Math.Pow(v1, v2),
                _ => 0
            };

            if (operation == "Разделить (/)" && v2 == 0)
                return View(new DataViewModel { ErrorMessage = "Деление на ноль." });

            return View(new DataViewModel { Result = Math.Round(result, 4) });
        }
    }
}