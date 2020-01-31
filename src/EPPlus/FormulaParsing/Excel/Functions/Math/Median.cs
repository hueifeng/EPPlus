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
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
    internal class Median : ExcelFunction
    {
        public override CompileResult Execute(IEnumerable<FunctionArgument> arguments, ParsingContext context)
        {
            var nums = ArgsToDoubleEnumerable(arguments, context);
            var arr = nums.ToArray();
            Array.Sort(arr);
            ThrowExcelErrorValueExceptionIf(() => arr.Length == 0, eErrorType.Num);
            double result;
            if (arr.Length % 2 == 1)
            {
                result = arr[arr.Length / 2];
            }
            else
            {
                var startIndex = arr.Length/2 - 1;
                result = (arr[startIndex] + arr[startIndex + 1])/2d;
            }
            return CreateResult(result, DataType.Decimal);
        }
    }
}