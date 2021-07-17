using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebAppSample1.Models;

namespace WebAppSample1.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IRegisterUserDAL _registeruser;
        public ClientesController( IRegisterUserDAL user)
        {
            _registeruser = user;
        }

        // GET: Clientes
        public ActionResult Index()
        {
            List<RegisterUsers> list = new List<RegisterUsers>();
            list = _registeruser.GetUsers(1, "").ToList();
            return View(list);
        }

        // GET: Clientes/Details/5
        public ActionResult Detalhes(int id)
        {
            RegisterUsers record = _registeruser.Select(id);
            List<ItemValue> ecv = _registeruser.GetEcvList();
            ViewBag.EcvList = new SelectList(ecv, "Id", "Text", record.CODECV);
            List<TextValue> gen = _registeruser.GetGENList();
            ViewBag.GenList = new SelectList(gen, "Id", "Text", record.TIPPES);
            List<TextValue> ufe = _registeruser.GetUFEList();
            ViewBag.UfeList = new SelectList(ufe, "Id", "Text", record.UFEEMI);
            return View(record);
        }

        // GET: Clientes/Create
        public ActionResult Add()
        {
            List<ItemValue> ecv = _registeruser.GetEcvList();
            ViewBag.EcvList = new SelectList(ecv, "Id", "Text", 0);
            List<TextValue> gen = _registeruser.GetGENList();
            ViewBag.GenList = new SelectList(gen, "Id", "Text", 0);
            List<TextValue> ufe = _registeruser.GetUFEList();
            ViewBag.UfeList = new SelectList(ufe, "Id", "Text", 0);

            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add([Bind] RegisterUsers record)
        {
            if (ModelState.IsValid)
            {
                _registeruser.Insert(record);
                return RedirectToAction("Index");
            }
            return View(record);
        }


        // GET: Clientes/Edit/5
        public ActionResult Edit(int id)
        {
            RegisterUsers record = _registeruser.Select(id);
            List<ItemValue> ecv = _registeruser.GetEcvList();
            ViewBag.EcvList = new SelectList(ecv, "Id", "Text", record.CODECV);
            List<TextValue> gen = _registeruser.GetGENList();
            ViewBag.GenList = new SelectList(gen, "Id", "Text", record.TIPPES);
            List<TextValue> ufe = _registeruser.GetUFEList();
            ViewBag.UfeList = new SelectList(ufe, "Id", "Text", record.UFEEMI);
            return View(record);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind]RegisterUsers record)
        {
            if (id != record.CODUSU)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _registeruser.Update(record);
                return RedirectToAction("Index");
            }
            return View(record);
        }
    }
}