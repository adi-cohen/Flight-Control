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
            IdNumber generated_id, ret;

            do
            {
                generated_id = new IdNumber((long)rand.Next(10000, 999999999));
                // Check if the new ID already exists.
                ret = _db.IdNumbers.SingleOrDefault(c => c.Id == generated_id.Id);

            } while (ret != null);

            // Add to DB.
            _db.IdNumbers.Add(generated_id);
            return generated_id.Id.ToString();
        }
    }

}
