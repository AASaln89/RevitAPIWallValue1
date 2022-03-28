using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIWallVolume
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementsRefList = uidoc.Selection.PickObjects(ObjectType.Face, "Select elem");
            var elementList = new List<Element>();
            double sumVolume = 0;
            foreach (var selectedElement in selectedElementsRefList)
            {
                Element element = doc.GetElement(selectedElement);
                if (element is Wall)
                {
                    Parameter volumeParameter = element.LookupParameter("Volume");
                    if (volumeParameter.StorageType == StorageType.Double)
                    {
                        double volumeValue = UnitUtils.ConvertFromInternalUnits(volumeParameter.AsDouble(), UnitTypeId.CubicMeters);
                        sumVolume += volumeValue;
                    }
                }
            }
            TaskDialog.Show("Сообщение", sumVolume.ToString());
            return Result.Succeeded;
        }
    }
}
