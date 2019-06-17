using Invent.Common.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Invent.Common.Services
{
    public class DbService
    {
        string dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "inventdatabase.db3");


        public Response StoreDB<T>(
                  List<T> list)
        {
            try
            {
                int rowsAdded = 0;

                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    conn.CreateTable<T>();

                    conn.DeleteAll<T>();

                    foreach (var item in list)
                    {
                        rowsAdded += conn.Insert(item);
                    }                
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = $"{rowsAdded} rows added to database",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public Response StoreDBItem<T>(
                  T item)
        {
            try
            {

                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    conn.CreateTable<T>();

                    conn.DeleteAll<T>();

                    conn.Insert(item);
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = $"Item added to database",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public Response GetDB<T>() where T : new()
        {
            var items = new List<T>();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    items = conn.Table<T>().ToList();
                }

                return new Response
                {
                    IsSuccess = true,
                    Result = items
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public Response UpdateDB<T>(
                  List<T> list)
        {
            try
            {
                int rowsChanged = 0;

                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    conn.CreateTable<T>();

                    foreach (var item in list)
                    {
                        rowsChanged += conn.Update(item);
                    }
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = $"{rowsChanged} rows changed in the database",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
