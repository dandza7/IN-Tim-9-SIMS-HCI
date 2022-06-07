﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Model.Preview;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class InventoryController
    {
        public InventoryService _inventoryService;

        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public List<InventoryPreview> GetPreviews()
        {
            return _inventoryService.GetPreviews().ToList();
        }
        
        public List<string> GetSOPRooms()
        {
            return _inventoryService.GetSOPRooms();
        }
        public Inventory GetById(int id)
        {
            return _inventoryService.GetById(id);
        }
        public Inventory Create(Inventory inv, string roomName)
        {
            return _inventoryService.Create(inv, roomName);
        }
        public IEnumerable<Inventory> GetAllDynamic()
        {
            return _inventoryService.GetAllDynamic();
        }

    }
}
