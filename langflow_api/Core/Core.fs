namespace langflow_api

module Core = 
   
   open System
   open System.Text
   open System.Text.Json
   open System.Text.Json.Serialization

   //parse the request
    let parse_instructions_request (request : string) : bool * string  =

        use jsondoc = JsonDocument.Parse request
        let root = jsondoc.RootElement

        let mutable instructions = " "
        let mutable element : JsonElement = root

        let result = root.TryGetProperty("instructions", &element)
        
        if result = true then 
            instructions <- element.GetString()
            (true, instructions)
        else
            (false, "instructions not included in the body of the json request")