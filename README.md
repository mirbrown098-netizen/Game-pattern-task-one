 Game Pattern Task One

This Unity project demonstrates the Singleton and Observer design patterns working together in one scene.

## Singleton Pattern
The GameManager and AudioManager use the Singleton pattern. This allows important game systems to be accessed from one place.

## Observer Pattern
The health and score systems use events to notify the UI when something changes. The health text updates when the player takes damage, and the score text updates when the score increases.

## Integration
The patterns work together in the same scene. Player health, score, UI, and audio respond to changes during gameplay.

## Testing
The test commands were used to:
- Decrease player health
- Increase the score
- Play audio
- Reload the scene

## AI Attribution
ChatGPT was used to help explain the assignment requirements, troubleshoot Unity errors,
