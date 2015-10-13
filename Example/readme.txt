Het bestand 'timeline.json' bevat de timeline: Het geeft aan wanneer welke commercial getoond wordt. Het bevat de volgende velden:

- name: Naam van de timeline.
- defaultDuration: Standaard duur dat een commercial getoong wordt in seconden. Deze waarde wordt gebruikt als er in een slot geen specifieke waarde wordt gegegevwn.
- slots: verzameling van slots. Elke slot geeft aan welke commercial wanneer wordt afgespeeld

Per slot:
- commercial: Naam van de jpg/png file die gettond moet worden. Files dienen te staan in dezelfde folder als waar 'timeline.json' staat
- start: Start van de commercial relatief tov dat de commercials xaml wordt gestart in vmix. Formaat is minuten:seconden
- (optioneel) duration: Tijd dat de commercial getoond moet worden in seconden. Als deze waarde wordt weggelaten, wordt de default duration gebruikt.

In de file commercial.settings (staat in dezelfde folder waar de commercials XAML is geinstalleerd) kan gespecifieerd worden waar de timeline.json gevonden kan worden.


Oktober 2015
Theo van Drimmelen.