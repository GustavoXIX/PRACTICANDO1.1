using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Practica1._1
{
    public partial class Practicando : Form
    {
        // XML path, que te dan para resolver
        string pathXML = ConfigurationManager.AppSettings["XML_UNIZAR"];
        // La conexion a la Bases Dastos
        string conexionBD = ConfigurationManager.AppSettings["DBConnection"];
        // La ruta, del xml ya serializado.
        string pathSerialize = ConfigurationManager.AppSettings["PathSerialize"];
        // Tabla para dar formato a el DataGrewView.
        DataTable tabla;
        // XDocument, para darle formato a el XML.
        XDocument MyDoc;
        // XDocument, para darle formato a el XML.
        XDocument doc;
        public Practicando()
        {
            InitializeComponent();
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var path = openFileDialog1.FileName;

                XDocument doc = XDocument.Load(path);

                txtMostrar1.Text = doc.ToString();

                foreach (var Row in doc.Root.Descendants("Row"))
                {
                    insertBD(Row);
                }

            }
        }
        private void insertBD(XElement Row)
        {
            string conexionBD = ConfigurationManager.AppSettings["DBConnection"];

            var qry = @"INSERT INTO [dbo].[MATRICULA_UNIZAR_GUSTAVO_DISLA] ([CURSO_ACADEMICO],[LOCALIDAD],[TIPO_CENTRO],[CENTRO],[ESTUDIO],[TIPO_ESTUDIO]
                     ,[NOMBRE_CCAA_ALUMNO],[SEXO],[MOVILIDAD_SALIDA],[DEDICACION],[ALUMNOS_MATRICULADOS],[FECHA_ACTUALIZACION])
                       VALUES
                       (@CURSO_ACADEMICO,@LOCALIDAD,@TIPO_CENTRO,@CENTRO
                       ,@ESTUDIO,@TIPO_ESTUDIO,@NOMBRE_CCAA_ALUMNO
                       ,@SEXO,@MOVILIDAD_SALIDA,@DEDICACION,@ALUMNOS_MATRICULADOS
                       ,@FECHA_ACTUALIZACION)";

            using SqlConnection con = new SqlConnection(conexionBD);
            SqlCommand cmd = new SqlCommand(qry, con);


            cmd.Parameters.AddWithValue("CURSO_ACADEMICO", Row.Element("CURSO_ACADEMICO").Value);
            cmd.Parameters.AddWithValue("LOCALIDAD", Row.Element("LOCALIDAD").Value);
            cmd.Parameters.AddWithValue("TIPO_CENTRO", Row.Element("TIPO_CENTRO").Value);
            cmd.Parameters.AddWithValue("CENTRO", Row.Element("CENTRO").Value);
            cmd.Parameters.AddWithValue("ESTUDIO", Row.Element("ESTUDIO").Value);
            cmd.Parameters.AddWithValue("TIPO_ESTUDIO", Row.Element("TIPO_ESTUDIO").Value);
            cmd.Parameters.AddWithValue("NOMBRE_CCAA_ALUMNO", Row.Element("NOMBRE_CCAA_ALUMNO").Value);
            cmd.Parameters.AddWithValue("SEXO", Row.Element("SEXO").Value);
            cmd.Parameters.AddWithValue("MOVILIDAD_SALIDA", Row.Element("MOVILIDAD_SALIDA").Value);
            cmd.Parameters.AddWithValue("DEDICACION", Row.Element("DEDICACION").Value);
            cmd.Parameters.AddWithValue("ALUMNOS_MATRICULADOS", Row.Element("ALUMNOS_MATRICULADOS").Value);
            cmd.Parameters.AddWithValue("FECHA_ACTUALIZACION", Row.Element("FECHA_ACTUALIZACION").Value);

            con.Open();
            cmd.ExecuteNonQuery();
        }
        private void btnReader_Click(object sender, EventArgs e)
        {
            string conexionBD = ConfigurationManager.AppSettings["DBConnection"];
            var qry = @"select 
                        [CURSO_ACADEMICO]
                      ,[LOCALIDAD]
                      ,[TIPO_CENTRO]
                      ,[CENTRO]
                      ,[ESTUDIO]
                      ,[TIPO_ESTUDIO]
                      ,[NOMBRE_CCAA_ALUMNO]
                      ,[SEXO]
                      ,[MOVILIDAD_SALIDA]
                      ,[DEDICACION]
                      ,[ALUMNOS_MATRICULADOS]
                      ,[FECHA_ACTUALIZACION]
	                  from MATRICULA_UNIZAR_GUSTAVO_DISLA;";


            using SqlConnection con = new SqlConnection(conexionBD);
            SqlCommand cmd = new SqlCommand(qry, con);
            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                XDocument document = new XDocument
                    (
                        new XElement("Rows",
                            new XElement("Row",
                                new XElement("CURSO_ACADEMICO", (int)reader["CURSO_ACADEMICO"]),
                                new XElement("LOCALIDAD", (string)reader["LOCALIDAD"]),
                                new XElement("TIPO_CENTRO", (string)reader["TIPO_CENTRO"]),
                                new XElement("CENTRO", (string)reader["CENTRO"]),
                                new XElement("ESTUDIO", (string)reader["ESTUDIO"]),
                                new XElement("TIPO_ESTUDIO", (string)reader["TIPO_ESTUDIO"]),
                                new XElement("NOMBRE_CCAA_ALUMNO", (string)reader["NOMBRE_CCAA_ALUMNO"]),
                                new XElement("SEXO", (string)reader["SEXO"]),
                                new XElement("MOVILIDAD_SALIDA", (string)reader["MOVILIDAD_SALIDA"]),
                                new XElement("DEDICACION", (string)reader["DEDICACION"]),
                                new XElement("ALUMNOS_MATRICULADOS", (string)reader["ALUMNOS_MATRICULADOS"]),
                                new XElement("FECHA_ACTUALIZACION", (DateTime)reader["FECHA_ACTUALIZACION"])
                          )
                      )
                   );
                txtMostrar1.AppendText(document.ToString());
            }
        }
        private void btnSerializer_Click(object sender, EventArgs e)
        {
            var qry = @"SELECT [CURSO_ACADEMICO]
                      ,[LOCALIDAD]
                      ,[TIPO_CENTRO]
                      ,[CENTRO]
                      ,[ESTUDIO]
                      ,[TIPO_ESTUDIO]
                      ,[NOMBRE_CCAA_ALUMNO]
                      ,[SEXO]
                      ,[MOVILIDAD_SALIDA]
                      ,[DEDICACION]
                      ,[ALUMNOS_MATRICULADOS]
                      ,[FECHA_ACTUALIZACION]
                      FROM [dbo].[MATRICULA_UNIZAR_GUSTAVO_DISLA]";

            string conexionBD = ConfigurationManager.AppSettings["DBConnection"];

            SqlConnection con = new SqlConnection(conexionBD);
            con.Open();

            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            List<UNIZAR> unizar = new List<UNIZAR>();

            while (reader.Read())
            {
                UNIZAR uni = new UNIZAR
                {
                    CURSO_ACADEMICO = (int)reader["CURSO_ACADEMICO"],
                    LOCALIDAD = (string)reader["LOCALIDAD"].ToString(),
                    TIPO_CENTRO = (string)reader["TIPO_CENTRO"].ToString(),
                    CENTRO = (string)reader["CENTRO"].ToString(),
                    ESTUDIO = (string)reader["ESTUDIO"].ToString(),
                    TIPO_ESTUDIO = (string)reader["TIPO_ESTUDIO"].ToString(),
                    NOMBRE_CCAA_ALUMNO = (string)reader["NOMBRE_CCAA_ALUMNO"].ToString(),
                    SEXO = (string)reader["SEXO"].ToString(),
                    MOVILIDAD_SALIDA = (string)reader["MOVILIDAD_SALIDA"].ToString(),
                    DEDICACION = (string)reader["DEDICACION"].ToString(),
                    ALUMNOS_MATRICULADOS = (string)reader["ALUMNOS_MATRICULADOS"].ToString(),
                    FECHA_ACTUALIZACION = (DateTime)reader["FECHA_ACTUALIZACION"],

                };

                unizar.Add(uni);
            }
            /* Serializa, los datos obtenidos de la lista; creada anterior mente y al que con el enum
            / se le había inntroducido los datos.*/
            XmlSerializer serializer = new XmlSerializer(typeof(List<UNIZAR>), new XmlRootAttribute("Row").ToString());
                                                                                   //XmlConvert().ToString();

            // TextWriter: es una clase abstracta. Por lo tanto, no se crea una instancia en el código. 
            // StreamWriter: objeto para escribir un archivo que enumera los directorios en la unidad C 
            using TextWriter writer = new StreamWriter(@"./UNIZAR.xml");
            // El objecto a serializar, en éste caso -unizar-
            serializer.Serialize(writer, unizar);
            MessageBox.Show("Todo Correcto", Text, MessageBoxButtons.OK);
        }
        private void EjecutarQUERY(string qry, string conexionBD)
        {
            SqlConnection con = new SqlConnection(conexionBD);
            con.Open();

            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();

            tabla = new DataTable();
            tabla.Load(reader);
            dg.DataSource = null;
            dg.DataSource = tabla;
        }
        private void btnEj0_Click(object sender, EventArgs e)
        {
            var qry = @"select * from MATRICULA_UNIZAR_GUSTAVO_DISLA";
            EjecutarQUERY(qry,conexionBD);
        }
        private void btnEj1_Click(object sender, EventArgs e)
        {
            var qry = @"select sum(ALUMNOS_MATRICULADOS) as Total_alumnos_matriculados
                        from MATRICULA_UNIZAR_GUSTAVO_DISLA;";
            EjecutarQUERY(qry, conexionBD);
        }
        private void btnEj2_Click(object sender, EventArgs e)
        {
            var qry = @"select SEXO, sum(ALUMNOS_MATRICULADOS) as TOTAL_POR_SEXO
                        from MATRICULA_UNIZAR_GUSTAVO_DISLA
                        GROUP BY SEXO;";
            EjecutarQUERY(qry,conexionBD);
        }
        private void btnEj3_Click(object sender, EventArgs e)
        {
            var qry = @"select top(5) ESTUDIO, sum(ALUMNOS_MATRICULADOS) as NUMERO_DE_ALUMNOS
                        from MATRICULA_UNIZAR_GUSTAVO_DISLA
                        where TIPO_ESTUDIO LIKE 'Máster'
                        group by ESTUDIO
                        order by 2 desc";
            EjecutarQUERY(qry,conexionBD);
        }
        private void btnEj4_Click(object sender, EventArgs e)
        {
            var qry = @"SELECT TOP(5)M.CENTRO, M.Mujeres * 100.0/ (M.Mujeres+H.Hombres)Porcentaje, M.Mujeres, H.Hombres
                        FROM(SELECT  CENTRO, SUM([ALUMNOS_MATRICULADOS]) Mujeres 
                        FROM [PRO].[dbo].[MATRICULA_UNIZAR_GUSTAVO_DISLA]
                        WHERE TIPO_ESTUDIO = 'Grado' AND SEXO = 'Mujeres'
                        GROUP BY CENTRO )M
                        LEFT JOIN 
	                        (SELECT  CENTRO, SUM([ALUMNOS_MATRICULADOS]) Hombres 
	                        FROM [PRO].[dbo].[MATRICULA_UNIZAR_GUSTAVO_DISLA]
	                        WHERE TIPO_ESTUDIO = 'Grado' AND SEXO = 'Hombres'
	                        GROUP BY CENTRO) H
                        ON M.CENTRO = H.CENTRO
                        ORDER BY 2 DESC;";

            EjecutarQUERY(qry,conexionBD);
        }
        private void btnEj5_Click(object sender, EventArgs e)
        {
            var qry = @"select DISTINCT  ESTUDIO, LOCALIDAD
                        from MATRICULA_UNIZAR_GUSTAVO_DISLA
                        where ESTUDIO IN(
                        SELECT TOP(5) ESTUDIO
                        FROM MATRICULA_UNIZAR_GUSTAVO_DISLA
                        WHERE TIPO_ESTUDIO like 'Grado' OR TIPO_ESTUDIO like 'Máster'
                        group by ESTUDIO
                        order by count(DISTINCT LOCALIDAD) desc);";
            EjecutarQUERY(qry,conexionBD);
        }
        private void btnDELTE_Click(object sender, EventArgs e)
        {
            var qry = @"DELETE FROM MATRICULA_UNIZAR_GUSTAVO_DISLA";

            SqlConnection con = new SqlConnection(conexionBD);
            con.Open();

            SqlCommand cmd = new SqlCommand(qry,con);

            cmd.ExecuteNonQuery();

        }
        private void btnReaderSerializer_Click(object sender, EventArgs e)
        {
            try
            {
                // Cargar xml serializado, con formato
                MyDoc = XDocument.Load(pathSerialize);
                MyDoc.Save(pathSerialize);
                txtMostrar1.Text = MyDoc.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void btnPrueba1(object sender, EventArgs e)
        {
            //Muestra todos los alumnos matriculados que estudien en un Centro adscrito en Teruel.
            var qry = @"";
            EjecutarQUERY(qry,conexionBD);
        }
    }
}

