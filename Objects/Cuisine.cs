using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BestRestaurant.Objects
{
    public class Cuisine
    {
        private int _id;
        private string _name;

        public Cuisine(string CuisineName, int Id = 0)
        {
            _id = Id;
            _name = CuisineName;
        }

        public override bool Equals(System.Object otherCuisine)
        {
            if (!(otherCuisine is Cuisine))
            {
                return false;
            }
            else
            {
                Cuisine newCuisine = (Cuisine) otherCuisine;
                bool idEquality = this.GetId() == newCuisine.GetId();
                bool nameEquality = this.GetName() == newCuisine.GetName();
                return (idEquality && nameEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }

        public void SetName(string newName)
        {
            _name = newName;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetId(int newId)
        {
            _id = newId;
        }

        public int GetId()
        {
            return _id;
        }

        public static List<Cuisine> GetAll()
        {
            List<Cuisine> allCuisines = new List<Cuisine> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM cuisines;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int cuisineId = rdr.GetInt32(0);
                string cuisineName = rdr.GetString(1);
                Cuisine newCuisine = new Cuisine(cuisineName, cuisineId);
                allCuisines.Add(newCuisine);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return allCuisines;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO cuisines (name) OUTPUT INSERTED.id VALUES (@CuisineName)", conn);
            SqlParameter nameParameter = new SqlParameter();
            nameParameter.ParameterName = "@CuisineName";
            nameParameter.Value = this.GetName();
            cmd.Parameters.Add(nameParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }
            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
        }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM cuisines;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static Cuisine Find(int searchId)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM cuisines WHERE id = @CuisineId", conn);
            SqlParameter cuisineIdParameter = new SqlParameter();
            cuisineIdParameter.ParameterName = "@CuisineId";
            cuisineIdParameter.Value = searchId.ToString();
            cmd.Parameters.Add(cuisineIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundCuisineId = 0;
            string foundCuisineName = null;

            while(rdr.Read())
            {
                foundCuisineId = rdr.GetInt32(0);
                foundCuisineName = rdr.GetString(1);
            }
            Cuisine foundCuisine = new Cuisine(foundCuisineName, foundCuisineId);

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return foundCuisine;
        }
    }
}
