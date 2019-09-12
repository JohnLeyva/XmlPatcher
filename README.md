# XmlPatcher

@startuml

User --> Wiki

Wiki --> DiagramGenerator : Request

DiagramGenerator --> GitRepository : GetFile

DiagramGenerator --> PlantUml


@enduml

