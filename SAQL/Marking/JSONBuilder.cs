namespace SAQL.Marking
{
    public class JSONBuilder : MarkedDataBuilder
    {
        public string GetData()
        {
            if (_markedData != null)
            {
                JSONMarkedData json = _markedData as JSONMarkedData;
                if(json != null)
                {
                    return json.getJSON();
                }
            }
            return "";
        }
    }
}
