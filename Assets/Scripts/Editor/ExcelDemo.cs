using UnityEngine;
using UnityEditor;

public static class ExcelDemo {
    private const string kSampleExcelPath = "Xlsx/Sample.xlsx";

    [MenuItem("ExcelDemo/Read Sample Excel")]
    private static void ReadSampleExcel () {
        Excel excel;
        Debug.Log("EXCEL READING START");
        Debug.Log("==================");
        if (Excel.TryRead(kSampleExcelPath, out excel)) {
            foreach (var sheet in excel.Sheets) {
                foreach (var cell in sheet.cells) {
                    Debug.Log(string.Format("{0}:{1}:{2} value={3}", sheet.name, cell.row, cell.column, cell.value));
                }
            }
        }
        Debug.Log("==================");
        Debug.Log("EXCEL READING END");
    }
}