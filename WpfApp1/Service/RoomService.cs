using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class RoomService
    {
        public readonly RoomRepository _roomRepository;
        public readonly DoctorRepository _doctorRepository;
        public readonly InventoryMovingRepository _inventoryMovingRepository;
        public readonly InventoryRepository _inventoryRepository;
        public readonly RenovationRepository _renovationRepository;
        public readonly NotificationRepository _notificationRepository;
        public readonly AppointmentRepository _appointmentRepository;

        public RoomService(RoomRepository roomRepository, DoctorRepository doctorRepository, InventoryMovingRepository inventoryMovingRepository, 
                            InventoryRepository inventoryRepository, RenovationRepository renovationRepository,
                            NotificationRepository notificationRepository, AppointmentRepository appointmentRepository)
        {
            _roomRepository = roomRepository;
            _doctorRepository = doctorRepository;
            _inventoryMovingRepository = inventoryMovingRepository;
            _inventoryRepository = inventoryRepository;
            _renovationRepository = renovationRepository;
            _notificationRepository = notificationRepository;
            _appointmentRepository = appointmentRepository;

        }
        public List<Room> GetAll()
        {
            return _roomRepository.GetAll();
        }

        public Room GetById(int id)
        {
            return _roomRepository.Get(id);
        }

        public Room Create(Room room)
        {
            return _roomRepository.Create(room);
        }

        public Room Update(Room room)
        {
            Room oldRoom = this._roomRepository.Get(room.Id);
            if (oldRoom.Type.Equals("Office") && !room.Type.Equals("Office"))
            {
                //---------------------------------------------------------------------------------
                //  MOVING ALL DOCTORS FROM EDITED ROOM TO MAIN OFFICE
                //---------------------------------------------------------------------------------
                List<Doctor> doctors = this._doctorRepository.GetAll().ToList();
                foreach (Doctor doctor in doctors)
                {
                    if (doctor.RoomId == room.Id)
                        doctor.RoomId = 2; // ID 2 is ID of Main Office and this room cannot be deleted
                }
                _doctorRepository.UpdateAll(doctors);
            }
            //  Checking if old room type is storage or operating (since only there static inventory can be stored) and checking that new room type is not one of those
            if ((oldRoom.Type.Equals("Storage") || oldRoom.Type.Equals("Operating")) && !(room.Type.Equals("Storage") || room.Type.Equals("Operating"))){
                List<int> DeletingQueue = new List<int>();
                //---------------------------------------------------------------------------------
                //  CANCELING ALL INVENTORY MOVINGS TO THIS ROOM
                //---------------------------------------------------------------------------------
                List<InventoryMoving> inventoryMovings = this._inventoryMovingRepository.GetAll().ToList();
                foreach (InventoryMoving inventoryMoving in inventoryMovings)
                {
                    if (inventoryMoving.RoomId == room.Id)
                        DeletingQueue.Add(inventoryMoving.Id);
                }
                foreach (int del in DeletingQueue)
                {
                    this._inventoryMovingRepository.Delete(del);
                }
                DeletingQueue.Clear();
                //---------------------------------------------------------------------------------
                //  MOVING ALL INVENTORY FROM EDITED ROOM TO MAIN STORAGE
                //---------------------------------------------------------------------------------
                List<Inventory> inventories = this._inventoryRepository.GetAll().ToList();
                foreach (Inventory inventory in inventories)
                {
                    if (inventory.RoomId == room.Id)
                        inventory.RoomId = 1; // ID 1 is ID of Main Storage, and this room cannot be deleted
                }
                _inventoryRepository.UpdateAll(inventories);
            }
            return _roomRepository.Update(room);
        }

        public bool Delete(int id)
        {
            List<int> DeletingQueue = new List<int>();
            //---------------------------------------------------------------------------------
            //  DELETING APPOINTMENTS THAT WERE IN THIS ROOM
            //---------------------------------------------------------------------------------
            List<Appointment> appointments = this._appointmentRepository.GetAll().ToList();
            foreach(Appointment appointment in appointments)
            {
                if(appointment.RoomId == id)
                    DeletingQueue.Add(appointment.Id);
            }
            foreach(int del in DeletingQueue)
            {
                this._appointmentRepository.Delete(del);
            }
            DeletingQueue.Clear();

            //---------------------------------------------------------------------------------
            //  DELETING RENOVATIONS THAT WERE IN THIS ROOM
            //---------------------------------------------------------------------------------
            List<Renovation> renovations = this._renovationRepository.GetAll().ToList();
            foreach (Renovation renovation in renovations)
            {
                if (renovation.RoomId == id)
                    DeletingQueue.Add(renovation.Id);
            }
            foreach (int del in DeletingQueue)
            {
                this._renovationRepository.Delete(del);
            }
            DeletingQueue.Clear();

            //---------------------------------------------------------------------------------
            //  CANCELING ALL INVENTORY MOVINGS TO THIS ROOM
            //---------------------------------------------------------------------------------
            List<InventoryMoving> inventoryMovings = this._inventoryMovingRepository.GetAll().ToList();
            foreach (InventoryMoving inventoryMoving in inventoryMovings)
            {
                if (inventoryMoving.RoomId == id)
                    DeletingQueue.Add(inventoryMoving.Id);
            }
            foreach (int del in DeletingQueue)
            {
                this._inventoryMovingRepository.Delete(del);
            }
            DeletingQueue.Clear();

            //---------------------------------------------------------------------------------
            //  MOVING ALL INVENTORY FROM DELETED ROOM TO MAIN STORAGE
            //---------------------------------------------------------------------------------
            List<Inventory> inventories = this._inventoryRepository.GetAll().ToList();
            foreach (Inventory inventory in inventories)
            {
                if (inventory.RoomId == id)
                    inventory.RoomId = 1; // ID 1 is ID of Main Storage and this room cannot be deleted
            }
            _inventoryRepository.UpdateAll(inventories);

            //---------------------------------------------------------------------------------
            //  MOVING ALL DOCTORS FROM DELETED ROOM TO MAIN OFFICE
            //---------------------------------------------------------------------------------
            List<Doctor> doctors = this._doctorRepository.GetAll().ToList();
            foreach (Doctor doctor in doctors)
            {
                if (doctor.RoomId == id)
                    doctor.RoomId = 2; // ID 2 is ID of Main Office, and this room cannot be deleted
            }
            _doctorRepository.UpdateAll(doctors);


            return _roomRepository.Delete(id);
        }
        public int GetIdByNametag(string nametag)
        {
            List<Room> rooms = _roomRepository.GetAll();
            foreach(Room room in rooms)
            {
                if (room.Nametag.Equals(nametag))
                {
                    return room.Id;
                }
            }
            return -1;
        }

    }
}
