using App_practical.Models;
using App_practical.Services; 
using Confluent.Kafka; 
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json; 

namespace App_practical.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;
       
        private readonly KafkaProducerService<Null, string> _producer;

       
        public HomeController(DatabaseContext context, KafkaProducerService<Null, string> producer)
        {
            _context = context;
            _producer = producer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var historial = _context.Variants.ToList();
            return View(historial);
        }

        [HttpPost]
        
        public async Task<IActionResult> Index(string value1, string value2, string operation)
        {
            if (!double.TryParse(value1.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double v1) ||
                !double.TryParse(value2.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double v2))
            {
                ViewBag.ErrorMessage = "Было введено неверное число.";
                return View(_context.Variants.ToList());
            }

            if (operation == "Разделить (/)" && v2 == 0)
            {
                ViewBag.ErrorMessage = "Деление на ноль.";
                return View(_context.Variants.ToList());
            }

          
            var nuevoCalculo = new Variant
            {
                Value1 = v1,
                Value2 = v2,
                Operation = operation
            };

            
            var json = JsonSerializer.Serialize(nuevoCalculo);
            await _producer.ProduceAsync("gonzalez", new Message<Null, string> { Value = json });

            
            return RedirectToAction("Index");
        }

        
        [HttpPost]
        public IActionResult Callback([FromBody] Variant variant)
        {
            if (variant != null)
            {
                _context.Variants.Add(variant);
                _context.SaveChanges();
            }
            return Ok();
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