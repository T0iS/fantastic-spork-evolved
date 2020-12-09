using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;
using BusinessLayer;

namespace DataLayer.Database.FunctionalityClasses
{
    public class MatchTable
    {
  
    public static String SQL_INSERT = "INSERT INTO Match (Id, MNUM, MATCHDATE, Win, Score, Game_id, Event_id) VALUES (@id, @num, @md, @Win, @score, @game_id, @event_id)";

    public static String SQL_UPDATE = "UPDATE Match SET MNUM=@num, MATCHDATE=@md, Win=@Win, Score=@score, " +
            "Game_id=@game_id, Event_id=@event_id WHERE Id=@id";
    public static String SQL_DELETE_ID = "DELETE FROM Match WHERE MNUM=@num";
    public static String SQL_SELECT = "SELECT * FROM Match";


    private static void PrepareCommand(SqlCommand command, Match m)
    {
        
        command.Parameters.AddWithValue("@id", m.Id);
        command.Parameters.AddWithValue("@num", m.MNUM);
        command.Parameters.AddWithValue("@md", m.MATCHDATE);
        command.Parameters.AddWithValue("@Win", m.Win);
        command.Parameters.AddWithValue("@score", m.Score);
        command.Parameters.AddWithValue("@game_id", m.Game_Id.Id);
        command.Parameters.AddWithValue("@event_id", m.Event_Id.Id);
        
    }

    private static Collection<Match> Read(SqlDataReader reader)
    {
        Collection<Match> Matches = new Collection<Match>();

        while (reader.Read())
        {
            int i = -1;
            Match m = new Match();
            m.Id = reader.GetInt32(++i);
            m.MNUM = reader.GetInt32(++i);
            m.MATCHDATE = reader.GetString(++i);
            m.Win = reader.GetString(++i);
            m.Score = reader.GetInt32(++i);

             
            Game g = new Game();
            g.Id = reader.GetInt32(++i);
            m.Game_Id = g;
            Event ev = new Event();
            ev.Id = reader.GetInt32(++i);
            m.Event_Id = ev;


            Matches.Add(m);
        }
        return Matches;
    }


    public static int Insert(Match m, DatabaseT pDb = null)
    {
        DatabaseT db = new DatabaseT();
        db.Connect();
        SqlCommand command = db.CreateCommand(SQL_INSERT);
        PrepareCommand(command, m);
        int ret = db.ExecuteNonQuery(command);
        db.Close();
        return ret;
    }


    public static int Update(Match m, DatabaseT pDb = null)
    {
        DatabaseT db = new DatabaseT();
        db.Connect();
        SqlCommand command = db.CreateCommand(SQL_UPDATE);
        PrepareCommand(command, m);
        int ret = db.ExecuteNonQuery(command);
        db.Close();
        return ret;
    }


    public static int Delete(int matchNumber, DatabaseT pDb = null)
    {
        DatabaseT db = new DatabaseT();
        db.Connect();
        SqlCommand command = db.CreateCommand(SQL_DELETE_ID);

        command.Parameters.AddWithValue("@num", matchNumber);
        int ret = db.ExecuteNonQuery(command);

        db.Close();
        return ret;
    }


    public static Collection<Match> Select(DatabaseT pDb = null)
    {
        DatabaseT db;
        if (pDb == null)
        {
            db = new DatabaseT();
            db.Connect();
        }
        else
        {
            db = (DatabaseT)pDb;
        }

        SqlCommand command = db.CreateCommand(SQL_SELECT);
        SqlDataReader reader = db.Select(command);

        Collection<Match> Users = Read(reader);
        reader.Close();

        if (pDb == null)
        {
            db.Close();
        }

        return Users;
    }


}
}
