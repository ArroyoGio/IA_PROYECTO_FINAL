# ARQUITECTURA DE NPCs - ECOSISTEMA SUBMARINO ALIENÍGENA

## JERARQUÍA CORRECTA

```
AICharacterControl (abstracta)
│
├── IACharacterAction (abstracta MonoBehaviour)
│   ├── PreyActionLand
│   │   ├── FishActionLand
│   │   └── DolphinActionLand
│   ├── PredatorActionLand
│   │   ├── SharkActionLand
│   │   ├── SnakeActionLand
│   │   └── KrakenActionLand
│   ├── SurvivorActionLand
│   │   ├── OctopusActionLand
│   │   └── LobsterActionLand
│   ├── HumanActionLand
│   │   └── DiverActionLand
│   └── AmbientActionLand
│       └── SphereActionLand
│
└── IACharacterVehicle (abstracta MonoBehaviour)
    ├── PreyVehicleLand
    │   ├── FishVehicleLand
    │   └── DolphinVehicleLand
    ├── PredatorVehicleLand
    │   ├── SharkVehicleLand
    │   ├── SnakeVehicleLand
    │   └── KrakenVehicleLand
    ├── SurvivorVehicleLand
    │   ├── OctopusVehicleLand
    │   └── LobsterVehicleLand
    ├── HumanVehicleLand
    │   └── DiverVehicleLand
    └── AmbientVehicleLand
        └── SphereVehicleLand

Health (abstracta)
├── HealthCombat
│   ├── SharkHealth
│   ├── DiverHealth
│   └── KrakenHealth
└── HealthNoCombat
    ├── FishHealth
    └── [DolphinHealth, OctopusHealth, etc.]

Blackboard (singelton)
AIEyeBase (abstracta)
└── AIEyeLand
```

## RESPONSABILIDADES

### AICharacterControl
- Referencia a Health, Blackboard, Eye, Agent
- Método abstracto UpdateAI()

### IACharacterAction & IACharacterVehicle
- Son **Componentes MonoBehaviour independientes**
- NO heredan de AICharacterControl
- Se comunican a través de Blackboard

### Health & Subclases
- Gestión de salud desacoplada
- Extensible para cada criatura

### Behavior Tree
- Sistema de decisiones con Selector/Sequence
- Nodes: ConditionNode, ActionNode
- Ejecuta a tasa configurable

## STEERING BEHAVIORS IMPLEMENTADOS
✓ Wander
✓ Pursuit  
✓ Evade
✓ Arrive
✓ Hide
- OffsetPursuit (opcional)

## PRÓXIMAS ETAPAS
1. Corregir referencias de namespaces
2. Completar NPCs concretos
3. Integrar Behavior Trees
4. Testear en escena
