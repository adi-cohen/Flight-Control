using System;
using System.Linq;


namespace FlightControlWeb.Models
{
    public class IdGenerator
    {

        private readonly DBInteractor _db;

        public IdGenerator(DBInteractor db)
        {
            _db = db;
        }

        // Generate random ID number string.
        public string GanerateID()
        {
            Random rand = new Random();
            IdNumber generated_id;

            do
            {
                generated_id = new IdNumber(rand.Next(10000, 999999999).ToString());
            } while (!isUnique(generated_id));

            // Add to DB.
            _db.IdNumbers.Add(generated_id);
            _db.SaveChanges();
            return generated_id.Id.ToString();
        }

        // Check if the new ID already exists.
        public bool isUnique(IdNumber num)
        {
            IdNumber ret = _db.IdNumbers.Find(num.Id);
            if (ret == null)
            {
                return true;
            }
            return false;
        }
    }

}
