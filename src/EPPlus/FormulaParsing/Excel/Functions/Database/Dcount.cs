/*************************************************************************************************
  Required Notice: Copyright (C) EPPlus Software AB. 
  This software is licensed under PolyForm Noncommercial License 1.0.0 
  and may only be used for noncommercial purposes 
  https://polyformproject.org/licenses/noncommercial/1.0.0/

  A commercial license to use this software can be purchased at https://epplussoftware.com
 *************************************************************************************************
  Date               Author                       Change
 *************************************************************************************************
  01/27/2020         EPPlus Software AB       Initial release EPPlus 5
 *************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.Utils;

namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Database
{
    public class Dcount : ExcelFunction
    {
        private readonly RowMatcher _rowMatcher;

        public Dcount()
            : this(new RowMatcher())
        {
            
        }

        public Dcount(RowMatcher rowMatcher)
        {
            _rowMatcher = rowMatcher;
        }

        public override CompileResult Execute(IEnumerable<FunctionArgument> arguments, ParsingContext context)
        {
            ValidateArguments(arguments, 2);
            var dbAddress = arguments.ElementAt(0).ValueAsRangeInfo.Address.Address;
            string field = null;
            string criteriaRange = null;
            if (arguments.Count() == 2)
            {
                criteriaRange = arguments.ElementAt(1).ValueAsRangeInfo.Address.Address;
            }
            else
            {
                field = ArgToString(arguments, 1).ToLower(CultureInfo.InvariantCulture);
                criteriaRange = arguments.ElementAt(2).ValueAsRangeInfo.Address.Address;
            } 
            var db = new ExcelDatabase(context.ExcelDataProvider, dbAddress);
            var criteria = new ExcelDatabaseCriteria(context.ExcelDataProvider, criteriaRange);

            var nHits = 0;
            while (db.HasMoreRows)
            {
                var dataRow = db.Read();
                if (_rowMatcher.IsMatch(dataRow, criteria))
                {
                    // if a fieldname is supplied, count only this row if the value
                    // of the supplied field is numeric.
                    if (!string.IsNullOrEmpty(field))
                    {
                        var candidate = dataRow[field];
                        if (ConvertUtil.IsNumericOrDate(candidate))
                        {
                            nHits++;
                        }
                    }
                    else
                    {
                        // no fieldname was supplied, always count matching row.
                        nHits++;    
                    }
                }
            }
            return CreateResult(nHits, DataType.Integer);
        }
    }
}