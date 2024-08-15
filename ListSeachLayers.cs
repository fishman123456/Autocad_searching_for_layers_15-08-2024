using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;

namespace Autocad_searching_for_layers_15_08_2024
{
    public class ListSeachLayers
    {
        [CommandMethod("U_83_ListLayers", CommandFlags.Redraw)]
        public void ListLay()
        {
            // проверяем по дате
            CheckDateWork.CheckDate();
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = HostApplicationServices.WorkingDatabase;
            var doc = Application.DocumentManager.MdiActiveDocument;
            Transaction tr = db.TransactionManager.StartTransaction();
            // выбираем все обьекты на чертеже
            TypedValue[] tvs = new TypedValue[2] {
                new TypedValue((int)DxfCode.Start, "INSERT"),
                new TypedValue((int)DxfCode.LayerName) };
            // фильтр выбора

            SelectionFilter sf = new SelectionFilter(tvs);

            PromptSelectionResult res = ed.SelectAll();
            // создаём массив всех обьектов, потом будем проверять по совпадению имени слоя
            ObjectId[] idArray = new ObjectId[2];

            SelectionSet selSet = res.Value;
            // проверка на пустоту чертежа
            if (selSet != null)
            {
                idArray = selSet.GetObjectIds();
            }
            else
            {
                Application.ShowAlertDialog("нет обьекта в чертеже");
                return;
            }
            // создаем список обьектов, потом в массив преобразуем и выделим в модели 24-07-2024
            List<Polyline3d> pid = new List<Polyline3d>();
            List<ObjectId> idPoly = new List<ObjectId>();
            // считываем таблицу слоев
            LayerTable tblLayer = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead, false);
            // пробегаем по всем элементам в таблиые слоёв
            int countLay = 0;
            //foreach (ObjectId ent in idArray)
            //{
                BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
                foreach (ObjectId id in btr)
                {
                    if (id.ObjectClass.Name == RXClass.GetClass(typeof(Polyline3d)).Name)
                    {
                        Polyline3d polyline3D = tr.GetObject(id, OpenMode.ForRead) as Polyline3d;
                        pid.Add(polyline3D);
                        idPoly.Add(id);
                        SelectionSet ss1 = SelectionSet.FromObjectIds(idPoly.ToArray());
                        ed.SetImpliedSelection(ss1.GetObjectIds());
                    countLay++;
                }
                }
               
            //}
            foreach (Polyline3d varible in pid)
            {
                ed.WriteMessage(varible.Layer + " - ");
                ed.WriteMessage(varible.Length + "\n");
            }
            ed.WriteMessage("количество кабеля = " +countLay + "\n");
        }
    }
}
