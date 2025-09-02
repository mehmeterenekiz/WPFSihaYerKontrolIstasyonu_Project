using SihaControlSystem.Classes.Parametreler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SihaControlSystem.Classes
{
    public class DBislemci
    {
        //DataGrid doldurma
        public static bool GridDoldur(DataGrid grd)
        {
            sbyte i = 0;
            SQLiteConnection con = new SQLiteConnection(DBconnection.DBaddress);
            //SQLiteCommand com = new SQLiteCommand("select gorev.ID, gorev.GorevAd,gorev.Tarih, gorev.SureDk, gorev.HedefMaxIrtifa, operator.AdiSoyadi as OperatorAdi, arac.HavaAraciAdi from tbl_Gorevler as gorev inner join tbl_Operatorler as operator on gorev.OperatorID = operator.ID inner join tbl_Araclar as arac on gorev.AracID = arac.ID", con);
            SQLiteCommand com = new SQLiteCommand("select * from tbl_Gorevler as gorev inner join tbl_Operatorler as operator on gorev.OperatorID = operator.ID inner join tbl_Araclar as arac on gorev.AracID = arac.ID", con);

            try
            {
                SQLiteDataAdapter adp = new SQLiteDataAdapter(com);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                grd.ItemsSource = null;
                grd.ItemsSource = dt.DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
            finally
            {
                con.Dispose();
            }

            if (i > 0) return true; else return false;
        }

        public static DataTable TumGorevKoordinatlariniGetir()
        {
            DataTable dt = new DataTable();

            using (SQLiteConnection con = new SQLiteConnection(DBconnection.DBaddress))
            {
                // Sadece koordinatları çekiyoruz
                string query = "select * from tbl_Gorevler as gorev inner join tbl_Operatorler as operator on gorev.OperatorID = operator.ID inner join tbl_Araclar as arac on gorev.AracID = arac.ID";

                using (SQLiteCommand com = new SQLiteCommand(query, con))
                {
                    try
                    {
                        SQLiteDataAdapter adp = new SQLiteDataAdapter(com);
                        adp.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veri çekme hatası: " + ex.Message);
                    }
                }
            }

            return dt;
        }

        //Task insert işlemi
        public static bool TaskInsert(Prm veri)
        {
            sbyte i = 0;


            using (SQLiteConnection con = new SQLiteConnection(DBconnection.DBaddress))
            {
                // Verileri inser edecek sorgu
                string query = "INSERT INTO tbl_Gorevler (GorevAd, Tarih, Saat, KoordinatLat, KoordinatLng, SureDk, HedefMaxIrtifa, OperatorID, AracID) VALUES (@GorevAd, @Tarih, @Saat, @KoordinatLat, @KoordinatLng, @SureDk, @HedefMaxIrtifa, @OperatorID, @AracID)";

                using (SQLiteCommand com = new SQLiteCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        com.Parameters.AddWithValue("@GorevAd", veri.GorevAd);
                        DateTime tarih = DateTime.Parse(veri.Tarih);  // Eğer veri.Tarih string ise
                        com.Parameters.AddWithValue("@Tarih", tarih.ToString("yyyy-MM-dd"));
                        com.Parameters.AddWithValue("@Saat", veri.Saat);
                        com.Parameters.AddWithValue("@KoordinatLat", veri.KoordinatLat);
                        com.Parameters.AddWithValue("@KoordinatLng", veri.KoordinatLng);
                        com.Parameters.AddWithValue("@SureDk", veri.SureDk);
                        com.Parameters.AddWithValue("@HedefMaxIrtifa", veri.HedefMaxIrtifa);
                        com.Parameters.AddWithValue("@OperatorID", veri.OperatorID);
                        com.Parameters.AddWithValue("@AracID", veri.AracID);
                        i = (sbyte)com.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veri ekleme hatası: " + ex.Message.ToString());
                    }
                    finally
                    {
                        con.Dispose();
                    }

                    if (i > 0) return true; else return false;

                }
            }
        }

        public static bool TaskDelete(int ID)
        {
            sbyte i = 0;

            using (SQLiteConnection con = new SQLiteConnection(DBconnection.DBaddress))
            {
                // Görevi delete edecek sorgu
                string query = "Delete FROM tbl_Gorevler where ID = @ID ";

                using (SQLiteCommand com = new SQLiteCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        com.Parameters.AddWithValue("@ID", ID);
                        i = (sbyte)com.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veri silme hatası: " + ex.Message.ToString());
                    }
                    finally
                    {
                        con.Dispose();
                    }

                    if (i > 0) return true; else return false;

                }
            }
        }

        public static Dictionary<string, object> TaskFetch(int ID)
        {
            // Verileri saklamak için bir Dictionary
            var taskData = new Dictionary<string, object>();

            using (SQLiteConnection con = new SQLiteConnection(DBconnection.DBaddress))
            {
                // Görevi seçecek sorgu
                string query = "SELECT * FROM tbl_Gorevler as gorev inner join tbl_Araclar as arac on arac.ID = gorev.AracID inner join tbl_Operatorler as operator on operator.ID = gorev.OperatorID WHERE gorev.ID = @ID";

                using (SQLiteCommand com = new SQLiteCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        com.Parameters.AddWithValue("@ID", ID);

                        using (SQLiteDataReader reader = com.ExecuteReader())
                        {
                            if (reader.Read()) // Eğer veri varsa
                            {
                                for (int i = 0; i < reader.FieldCount; i++) 
                                {
                                    string columnName = reader.GetName(i);
                                    object value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                                    taskData[columnName] = value;  // Sütun adını anahtar, değeri değer olarak ekle
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veri çekme hatası: " + ex.Message);
                    }
                    finally
                    {
                        con.Dispose();
                    }
                }
            }

            return taskData; // Dinamik veri döndür
        }

        public static bool TaskEdit(Prm veri, int ID)
        {
            sbyte i = 0;

            using (SQLiteConnection con = new SQLiteConnection(DBconnection.DBaddress))
            {
                // Verileri inser edecek sorgu
                string query = "UPDATE tbl_Gorevler SET GorevAd = @GorevAd, Tarih = @Tarih, Saat = @Saat, KoordinatLat = @KoordinatLat, KoordinatLng = @KoordinatLng, SureDk = @SureDk, HedefMaxIrtifa = @HedefMaxIrtifa, OperatorID = @OperatorID, AracID = @AracID WHERE ID = @ID";

                using (SQLiteCommand com = new SQLiteCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        com.Parameters.AddWithValue("@ID", ID);
                        com.Parameters.AddWithValue("@GorevAd", veri.GorevAd);
                        DateTime tarih = DateTime.Parse(veri.Tarih);  // Eğer veri.Tarih string ise
                        com.Parameters.AddWithValue("@Tarih", tarih.ToString("yyyy-MM-dd"));
                        com.Parameters.AddWithValue("@Saat", veri.Saat);
                        com.Parameters.AddWithValue("@KoordinatLat", veri.KoordinatLat);
                        com.Parameters.AddWithValue("@KoordinatLng", veri.KoordinatLng);
                        com.Parameters.AddWithValue("@SureDk", veri.SureDk);
                        com.Parameters.AddWithValue("@HedefMaxIrtifa", veri.HedefMaxIrtifa);
                        com.Parameters.AddWithValue("@OperatorID", veri.OperatorID);
                        com.Parameters.AddWithValue("@AracID", veri.AracID);
                        i = (sbyte)com.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veri ekleme hatası: " + ex.Message.ToString());
                    }
                    finally
                    {
                        con.Dispose();
                    }

                    if (i > 0) return true; else return false;

                }
            }
        }
    }
}
