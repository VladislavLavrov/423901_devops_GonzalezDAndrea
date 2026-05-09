using App_practical.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace App_practical.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;

        
        public HomeController(DatabaseContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public IActionResult Index()
        {
            var historial = _context.Variants.ToList();
            return View(historial);
        }

        [HttpPost]
        public IActionResult Index(string value1, string value2, string operation)
        {
            
            if (!double.TryParse(value1.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double v1) ||
                !double.TryParse(value2.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double v2))
            {
                ViewBag.ErrorMessage = "Было введено неверное число.";
                return View(_context.Variants.ToList());
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
            {
                ViewBag.ErrorMessage = "Деление на ноль.";
                return View(_context.Variants.ToList());
            }

            
            var nuevoCalculo = new Variant
            {
                Operand1 = v1,
                Operand2 = v2,
                Operation = operation,
                Result = Math.Round(result, 4)
            };

            _context.Variants.Add(nuevoCalculo); 
            _context.SaveChanges();             
            

            return RedirectToAction("Index"); 
        }

        
        public IActionResult Delete(int id)
        {
            var registro = _context.Variants.Find(id);
            if (registro != null)
            {
                 _context.Variants.Remove(registro); 
                 _context.SaveChanges(); 
            }
            return RedirectToAction("Index");
        }
    }
}