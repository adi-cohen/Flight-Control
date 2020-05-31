using System;
using System.Linq;


namespace FlightControlWeb.Models
{
    internal class IdGenerator
    {

        private readonly DBInteractor db;

        internal IdGenerator(DBInteractor db)
        {
            this.db = db;
        }

        // Generate random ID number string.
        internal string GanerateId()
        {
            var rand = new Random();
            IdNumber generatedID;

            do
            {
                generatedID = new IdNumber(rand.Next(10000, 999999999).ToString());
            } while (!IsUnique(generatedID));

            // Add to DB.
            db.IdNumbers.Add(generatedID);
            db.SaveChanges();
            return generatedID.Id.ToString();
        }

        // Check if the new ID already exists.
        internal bool IsUnique(IdNumber num)
        {
            var ret = db.IdNumbers.Find(num.Id);
            if (ret == null)
            {
                return true;
            }
            return false;
        }
    }

}
