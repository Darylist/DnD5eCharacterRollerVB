Public Class Character
    Private race, class_, alignment, subrace, background, hitDie, size As String
    Private level, HP, profBonus, speed As Integer
    Private AbilityScores(5) As Integer
    Private Modifiers(5) As Integer
    Private savingThrows(5) As Integer
    Private skillBonuses(17) As Integer
    Private auditLog As String
    Private Acs(12) As String 'will contain 13 values. no armor, and then all the armors from top to bottom on page 145 of the handbook.
    Public Enum SkillList
        ACROBATICS
        ANIMAL_HANDLING
        ARCANA
        ATHLETICS
        DECEPTION
        HISTORY
        INSIGHT
        INTIMIDATION
        INVESTIGATION
        MEDICINE
        NATURE
        PERCEPTION
        PERFORMANCE
        PERSUASION
        RELIGION
        SLEIGHT_OF_HAND
        STEALTH
        SURVIVAL
    End Enum

    Public Enum StatList
        Strength
        Dexterity
        Constitution
        Intelligence
        Wisdom
        Charisma
    End Enum

    Private Shared RacesList = New String() {
        "Dwarf",
        "Elf",
        "Halfling",
        "Human",
        "Dragonborn",
        "Gnome",
        "Half-Elf",
        "Half-Orc",
        "Tiefling"
    }

    Private Shared ClassesList = New String() {
        "Barbarian",
        "Bard",
        "Cleric",
        "Druid",
        "Fighter",
        "Monk",
        "Paladin",
        "Ranger",
        "Rogue",
        "Sorcerer",
        "Warlock",
        "Wizard"
   }

    Private Shared BackgroundsList = New String() {
        "Acolyte",
        "Charlatan",
        "Criminal",
        "Entertainer",
        "Folk Hero",
        "Guild Artisan",
        "Hermit",
        "Noble",
        "Outlander",
        "Sage",
        "Sailor",
        "Soldier",
        "Urchin"
    }

    Private Shared AlignmentsList = New String() {
        "Lawful Good",
        "Neutral Good",
        "Chaotic Good",
        "Lawful Neutral",
        "Neutral",
        "Chaotic Neutral",
        "Lawful Evil",
        "Neutral Evil",
        "Chaotic Evil"
    }
    'subrace arrays
    Private Shared subDwarf = New String() {"Hill Dwarf", "Mountain Dwarf"}
    Private Shared subElf = New String() {"High Elf", "Wood Elf", "Dark Elf"}
    Private Shared subHalfling = New String() {"Lightfoot", "Stout"}
    Private Shared subHuman = New String() {"Calishite", "Chondathan", "Damaran", "Illuskan", "Mulan", "Rashemi", "Shou", "Tethyrian", "Turami"}
    Private Shared subDragonborn = New String() {"Black", "Blue", "Brass", "Bronze", "Copper", "Gold", "Green", "Red", "Silver", "White"}
    Private Shared subGnome = New String() {"Forest Gnome", "Rock Gnome"}
    Private Shared subHalfElf = New String() {"N/A"}
    Private Shared subHalfOrc = New String() {"N/A"}
    Private Shared subTiefling = New String() {"N/A"}

    'Collect all subrace arrays in one jagged array of arrays, the index corresponds to the index of the races array
    'so if you have a combo box and the races are in the same order the index will match the first dimension of this index to pull in the subraces
    Private Shared subraces()() As String = {subDwarf, subElf, subHalfling, subHuman, subDragonborn, subGnome, subHalfElf, subHalfOrc, subTiefling}

    'functions to assist in populating dropdowns
    Public Shared Function GetRaces() As String()
        Return RacesList
    End Function
    Public Shared Function GetClasses() As String()
        Return ClassesList
    End Function
    Public Shared Function GetBackgrounds() As String()
        Return BackgroundsList
    End Function
    Public Shared Function GetAlignments() As String()
        Return AlignmentsList
    End Function
    Public Shared Function getSubraces() As String()()
        Return subraces
    End Function
    'constructor
    Public Sub New()
        'randomly pick a race, class and background at level 1
        auditLog = ""
        Randomize()
        Dim raceindex = CInt(Int(((RacesList.Length - 1) * Rnd())))
        race = RacesList(raceindex)
        auditLog = race & " Selected as race." & vbCrLf
        class_ = ClassesList(CInt(Int(((ClassesList.Length - 1) * Rnd()))))
        auditLog &= class_ & " Selected as class." & vbCrLf
        background = BackgroundsList(CInt(Int(((BackgroundsList.Length - 1) * Rnd()))))
        auditLog &= background & " Selected as background." & vbCrLf
        level = 1
        auditLog &= level & " Selected as level." & vbCrLf
        generateStats("")
    End Sub

    Public Sub rerollCharacter(ByVal r As String, ByVal c As String, ByVal b As String, ByVal s As String)
        'race class and background already determined.
        auditLog = ""
        race = r
        auditLog = race & " Selected as race." & vbCrLf
        class_ = c
        auditLog &= class_ & " Selected as class." & vbCrLf
        background = b
        auditLog &= background & " Selected as background." & vbCrLf
        level = 1
        auditLog &= level & " Selected as level." & vbCrLf
        generateStats(s)
    End Sub

    Private Sub selectSubRace(ByVal subr As String)
        'determine subrace
        'if subr is an empty string, choose a subrace
        If subr = "" Or subr = "Random" Then
            subrace = subraces(Array.IndexOf(RacesList, race))(CInt(Int(((subraces(Array.IndexOf(RacesList, race)).Length) * Rnd()))))
        Else
            subrace = subr
        End If

        auditLog &= subrace & " Selected as subrace." & vbCrLf
    End Sub

    Private Sub generateStats(ByVal subr As String)
        'set AbilityScores to -1
        AbilityScores = {-1, -1, -1, -1, -1, -1}
        selectSubRace(subr)
        'distribute abilityscores depending on the chosen class
        'make racial modifications to ability scores
        generateAbilityScores()
        'generate and set ability Modifier values
        setAbilityModifiers()
        'calculate HP and set hit die
        calculateHP()
        'Determine proficience bonus (2 for first level)
        profBonus = 2
        auditLog &= profBonus & " is the proficiency bonus." & vbCrLf
        'determine speed and size
        calculateSpeedSize()
        'calculate saving throws
        setSavingThrows()
        'determine skills
        determineBackgroundSkills()
        determineClassSkills()
        'calculate AC
        calculateAC()
    End Sub

    Private Sub generateAbilityScores()

        Dim unsortedScores(5) As Integer
        'roll 6 scores
        auditLog &= "Rolling 6 scores:"
        For x As Integer = 0 To 5
            unsortedScores(x) = rollAbility()
            auditLog &= unsortedScores(x) & " "
        Next
        auditLog &= vbCrLf
        'sort the scores (from lowest to highest)
        auditLog &= "Sorting the scores lowest to highest:"
        Array.Sort(unsortedScores)
        For x As Integer = 0 To 5
            auditLog &= unsortedScores(x) & " "
        Next
        auditLog &= vbCrLf & "Assigning Highest scores to primary stats:" & vbCrLf
        'highest scores will go into the characters primary abiliy (or abilities) designated by class
        Select Case class_
            Case "Barbarian", "Fighter"
                AbilityScores(StatList.Strength) = unsortedScores(5)
                auditLog &= unsortedScores(5) & " assigned to Strength" & vbCrLf
            Case "Bard", "Sorcerer", "Warlock"
                AbilityScores(StatList.Charisma) = unsortedScores(5)
                auditLog &= unsortedScores(5) & " assigned to Charisma" & vbCrLf
            Case "Cleric", "Druid"
                AbilityScores(StatList.Wisdom) = unsortedScores(5)
                auditLog &= unsortedScores(5) & " assigned to Wisdom" & vbCrLf
            Case "Wizard"
                AbilityScores(StatList.Intelligence) = unsortedScores(5)
                auditLog &= unsortedScores(5) & " assigned to Intelligence" & vbCrLf
            Case "Monk", "Ranger"
                AbilityScores(StatList.Dexterity) = unsortedScores(5)
                auditLog &= unsortedScores(5) & " assigned to Dexterity" & vbCrLf
                AbilityScores(StatList.Wisdom) = unsortedScores(4)
                auditLog &= unsortedScores(4) & " assigned to Wisdom" & vbCrLf
                unsortedScores(4) = -1
            Case "Paladin"
                AbilityScores(StatList.Strength) = unsortedScores(5)
                auditLog &= unsortedScores(5) & " assigned to Strength" & vbCrLf
                AbilityScores(StatList.Charisma) = unsortedScores(4)
                auditLog &= unsortedScores(4) & " assigned to Charisma" & vbCrLf
                unsortedScores(4) = -1
            Case "Rogue"
                AbilityScores(StatList.Dexterity) = unsortedScores(5)
                auditLog &= unsortedScores(5) & " assigned to Dexterity" & vbCrLf
                AbilityScores(StatList.Intelligence) = unsortedScores(4)
                auditLog &= unsortedScores(4) & " assigned to Intelligence" & vbCrLf
                unsortedScores(4) = -1
        End Select
        unsortedScores(5) = -1
        'by now the index 5 (and maybe 4) will be -1 to indicate those rolls were already used
        'now we take the remaining rolls and distribute them across the other abilities randomly
        'randomize the entire array (it's easier)
        RandomizeArray(unsortedScores)
        'go through the array, distributing the remainder of the scores
        'if the score is -1, skip it and go to the next score
        auditLog &= "Distributing the remainder of the stats" & vbCrLf
        Dim iterator = -1
        While iterator < 5
            iterator += 1
            If unsortedScores(iterator) = -1 Then
                Continue While
            End If

            If AbilityScores(StatList.Strength) < 1 Then
                AbilityScores(StatList.Strength) = unsortedScores(iterator)
                auditLog &= unsortedScores(iterator) & " assigned to Strength" & vbCrLf
                Continue While
            End If

            If AbilityScores(StatList.Dexterity) < 1 Then
                AbilityScores(StatList.Dexterity) = unsortedScores(iterator)
                auditLog &= unsortedScores(iterator) & " assigned to Dexterity" & vbCrLf
                Continue While
            End If

            If AbilityScores(StatList.Constitution) < 1 Then
                AbilityScores(StatList.Constitution) = unsortedScores(iterator)
                auditLog &= unsortedScores(iterator) & " assigned to Constitution" & vbCrLf
                Continue While
            End If

            If AbilityScores(StatList.Intelligence) < 1 Then
                AbilityScores(StatList.Intelligence) = unsortedScores(iterator)
                auditLog &= unsortedScores(iterator) & " assigned to Intelligence" & vbCrLf
                Continue While
            End If

            If AbilityScores(StatList.Wisdom) < 1 Then
                AbilityScores(StatList.Wisdom) = unsortedScores(iterator)
                auditLog &= unsortedScores(iterator) & " assigned to Wisdom" & vbCrLf
                Continue While
            End If

            If AbilityScores(StatList.Charisma) < 1 Then
                AbilityScores(StatList.Charisma) = unsortedScores(iterator)
                auditLog &= unsortedScores(iterator) & " assigned to Charisma" & vbCrLf
                Continue While
            End If

        End While
        'now all the base stats should be in place
        'the races get bonuses to certain stats. here they will be added
        auditLog &= "Distribute Racial Stat Bonuses" & vbCrLf
        'strength
        If race = "Dragonborn" Or race = "Half-Orc" Or subrace = "Mountain Dwarf" Then
            AbilityScores(StatList.Strength) += 2
            auditLog &= "Strength increased by 2 To " & AbilityScores(StatList.Strength) & vbCrLf
        End If

        If race = "Human" Then
            AbilityScores(StatList.Strength) += 1
            auditLog &= "Strength increased by 1 To " & AbilityScores(StatList.Strength) & vbCrLf
        End If
        'dexterity
        If race = "Elf" Or race = "Halfling" Then
            AbilityScores(StatList.Dexterity) += 2
            auditLog &= "Dexterity increased by 2 To " & AbilityScores(StatList.Dexterity) & vbCrLf
        End If

        If race = "Human" Or subrace = "Forest Gnome" Then
            AbilityScores(StatList.Dexterity) += 1
            auditLog &= "Dexterity increased by 1 To " & AbilityScores(StatList.Dexterity) & vbCrLf
        End If
        'constitution
        If race = "Dwarf" Then
            AbilityScores(StatList.Constitution) += 2
            auditLog &= "Constitution increased by 2 To " & AbilityScores(StatList.Constitution) & vbCrLf
        End If

        If race = "Human" Or race = "Half-Orc" Or subrace = "Stout" Or subrace = "Rock Gnome" Then
            AbilityScores(StatList.Constitution) += 1
            auditLog &= "Constitution increased by 1 To " & AbilityScores(StatList.Constitution) & vbCrLf
        End If
        'intelligence
        If race = "Gnome" Then
            AbilityScores(StatList.Intelligence) += 2
            auditLog &= "Intelligence increased by 2 To " & AbilityScores(StatList.Intelligence) & vbCrLf
        End If

        If race = "Human" Or race = "Tiefling" Or subrace = "High Elf" Then
            AbilityScores(StatList.Intelligence) += 1
            auditLog &= "Intelligence increased by 1 To " & AbilityScores(StatList.Intelligence) & vbCrLf
        End If
        'wisdom

        If race = "Human" Or subrace = "Wood Elf" Or subrace = "Hill Dwarf" Then
            AbilityScores(StatList.Wisdom) += 1
            auditLog &= "Wisdom increased by 1 To " & AbilityScores(StatList.Wisdom) & vbCrLf
        End If
        'charisma
        If race = "Half-Elf" Or race = "Tiefling" Then
            AbilityScores(StatList.Charisma) += 2
            auditLog &= "Charisma increased by 2 To " & AbilityScores(StatList.Charisma) & vbCrLf
        End If

        If race = "Human" Or race = "Dragonborn" Or subrace = "Dark Elf" Or subrace = "Lightfoot" Then
            AbilityScores(StatList.Charisma) += 1
            auditLog &= "Charisma increased by 1 To " & AbilityScores(StatList.Charisma) & vbCrLf
        End If

        'if Half-Elf need to increase 2 lowest stats other than charisma
        If race = "Half-Elf" Then
            'get the index of the lowest score
            Dim lowest1index, lowest2index As Integer
            Dim shortscores(4) As Integer
            'since we cant use charisma, copy the abilityscores to an array 1 smaller
            '(charisma is the last entry so it will maintain the ability score index)
            lowest1index = 6
            lowest2index = 7
            Array.Copy(AbilityScores, shortscores, 5)
            lowest1index = Array.IndexOf(shortscores, shortscores.Min)
            'lowest1index now contains lowest stat
            'now need 2nd lowest stat
            Dim index As Integer
            If lowest1index = 0 Then
                index = 1
            Else
                index = 0
            End If
            lowest2index = index
            While index < 4
                index += 1
                If index = lowest1index Then
                    Continue While
                End If
                If (shortscores(index) < shortscores(lowest2index)) Then
                    lowest2index = index
                End If

            End While
            'now should have the 2 lowest stat indexes

            auditLog &= "Half-Elf special case. 2 stats increase by 1 (not charisma) (im using lowest stats here) " & vbCrLf
            'add 1 to each
            AbilityScores(lowest1index) += 1
            auditLog &= [Enum].GetName(GetType(StatList), lowest1index) & " Increased by 1 to " & AbilityScores(lowest1index) & vbCrLf
            AbilityScores(lowest2index) += 1
            auditLog &= [Enum].GetName(GetType(StatList), lowest2index) & " Increased by 1 to " & AbilityScores(lowest2index) & vbCrLf
        End If
    End Sub

    Private Sub setAbilityModifiers()
        Dim index As Integer = 0
        While index <= 5
            Modifiers(index) = Math.Floor((AbilityScores(index) - 10) / 2)
            index += 1
        End While
    End Sub

    Private Sub RandomizeArray(ByVal items() As Integer)
        Dim max_index As Integer = items.Length - 1
        Dim rnd As New Random
        For i As Integer = 0 To max_index - 1
            ' Pick an item for position i.
            Dim j As Integer = rnd.Next(i, max_index + 1)

            ' Swap them.
            Dim temp As Integer = items(i)
            items(i) = items(j)
            items(j) = temp
        Next i
    End Sub

    Private Function rollAbility() As Integer
        Randomize()
        Dim total As Integer = 0
        Dim Rolls(3) As Integer
        'roll 4 random numbers between 1 and 6
        For y As Integer = 0 To 3
            Rolls(y) = CInt(Int((6 * Rnd()) + 1))
        Next
        'sort the numbers
        Array.Sort(Rolls)
        'add up the 3 highest numbers
        total = Rolls(1) + Rolls(2) + Rolls(3)
        'return the total of the 3 highest
        Return total
    End Function

    Private Sub calculateHP()
        'For now only calculating level 1
        Select Case class_
            Case "Barbarian"
                HP = 12 + getModifier(StatList.Constitution)
                hitDie = "1d12"
            Case "Bard", "Cleric", "Druid", "Monk", "Rogue", "Warlock"
                HP = 8 + getModifier(StatList.Constitution)
                hitDie = "1d8"
            Case "Fighter", "Paladin", "Ranger"
                HP = 10 + getModifier(StatList.Constitution)
                hitDie = "1d10"
            Case "Sorcerer", "Wizard"
                HP = 6 + getModifier(StatList.Constitution)
                hitDie = "1d6"
        End Select
    End Sub

    Private Sub calculateSpeedSize()
        Select Case race
            Case "Dwarf", "Halfling", "Gnome"
                speed = 25
                size = "Small"
            Case Else
                speed = 30
                size = "Medium"
        End Select
        'special case wood elf subrace has base speed of 35
        If subrace = "Wood Elf" Then speed += 5
    End Sub

    Private Sub setSavingThrows()
        savingThrows = {0, 0, 0, 0, 0, 0}
        Select Case class_
            Case "Barbarian", "Fighter"
                savingThrows(StatList.Strength) = profBonus
                savingThrows(StatList.Constitution) = profBonus
            Case "Bard"
                savingThrows(StatList.Dexterity) = profBonus
                savingThrows(StatList.Charisma) = profBonus
            Case "Cleric", "Paladin", "Warlock"
                savingThrows(StatList.Wisdom) = profBonus
                savingThrows(StatList.Charisma) = profBonus
            Case "Druid", "Wizard"
                savingThrows(StatList.Intelligence) = profBonus
                savingThrows(StatList.Wisdom) = profBonus
            Case "Monk", "Ranger"
                savingThrows(StatList.Dexterity) = profBonus
                savingThrows(StatList.Strength) = profBonus
            Case "Rogue"
                savingThrows(StatList.Dexterity) = profBonus
                savingThrows(StatList.Intelligence) = profBonus
            Case "Sorcerer"
                savingThrows(StatList.Charisma) = profBonus
                savingThrows(StatList.Constitution) = profBonus

        End Select
    End Sub

    Private Sub determineBackgroundSkills()
        skillBonuses = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Select Case background
            Case "Acolyte"
                skillBonuses(SkillList.INSIGHT) = profBonus
                skillBonuses(SkillList.RELIGION) = profBonus
            Case "Charlatan"
                skillBonuses(SkillList.DECEPTION) = profBonus
                skillBonuses(SkillList.SLEIGHT_OF_HAND) = profBonus
            Case "Criminal"
                skillBonuses(SkillList.DECEPTION) = profBonus
                skillBonuses(SkillList.STEALTH) = profBonus
            Case "Entertainer"
                skillBonuses(SkillList.ACROBATICS) = profBonus
                skillBonuses(SkillList.PERFORMANCE) = profBonus
            Case "Folk Hero"
                skillBonuses(SkillList.ANIMAL_HANDLING) = profBonus
                skillBonuses(SkillList.SURVIVAL) = profBonus
            Case "Guild Artisan"
                skillBonuses(SkillList.INSIGHT) = profBonus
                skillBonuses(SkillList.PERSUASION) = profBonus
            Case "Hermit"
                skillBonuses(SkillList.MEDICINE) = profBonus
                skillBonuses(SkillList.RELIGION) = profBonus
            Case "Noble"
                skillBonuses(SkillList.HISTORY) = profBonus
                skillBonuses(SkillList.PERSUASION) = profBonus
            Case "Outlander"
                skillBonuses(SkillList.ATHLETICS) = profBonus
                skillBonuses(SkillList.SURVIVAL) = profBonus
            Case "Sage"
                skillBonuses(SkillList.ARCANA) = profBonus
                skillBonuses(SkillList.HISTORY) = profBonus
            Case "Sailor"
                skillBonuses(SkillList.ATHLETICS) = profBonus
                skillBonuses(SkillList.PERCEPTION) = profBonus
            Case "Soldier"
                skillBonuses(SkillList.ATHLETICS) = profBonus
                skillBonuses(SkillList.INTIMIDATION) = profBonus
            Case "Urchin"
                skillBonuses(SkillList.SLEIGHT_OF_HAND) = profBonus
                skillBonuses(SkillList.STEALTH) = profBonus
        End Select
    End Sub

    Private Sub determineClassSkills()
        Dim numSkills As Integer = 0
        Dim skillOptions() As Integer = {}
        'this would take too long to type out all the options for each class, and I already had the list made
        'when I did this for java, the skills list enumeration pairs with the numbers used here.
        Select Case class_
            Case "Barbarian"
                skillOptions = {1, 3, 7, 10, 11, 17}
                numSkills = 2
            Case "Bard"
                skillOptions = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17}
                numSkills = 3
            Case "Cleric"
                skillOptions = {5, 6, 9, 13, 14}
                numSkills = 2
            Case "Druid"
                skillOptions = {2, 1, 6, 9, 10, 11, 14, 17}
                numSkills = 2
            Case "Fighter"
                skillOptions = {0, 1, 3, 5, 6, 7, 11, 17}
                numSkills = 2
            Case "Monk"
                skillOptions = {0, 3, 5, 6, 14, 16}
                numSkills = 2
            Case "Paladin"
                skillOptions = {3, 6, 7, 9, 13, 14}
                numSkills = 2
            Case "Ranger"
                skillOptions = {1, 3, 6, 8, 10, 11, 16, 17}
                numSkills = 3
            Case "Rogue"
                skillOptions = {0, 3, 4, 6, 7, 8, 11, 12, 13, 15, 16}
                numSkills = 4
            Case "Sorcerer"
                skillOptions = {2, 4, 6, 7, 13, 14}
                numSkills = 2
            Case "Warlock"
                skillOptions = {2, 4, 5, 7, 8, 10, 14}
                numSkills = 2
            Case "Wizard"
                skillOptions = {2, 5, 6, 8, 9, 14}
                numSkills = 2
        End Select
        Randomize()
        'in a loop, pick a random skill from the list for that class
        'if that skill is 0, add the profBonus to it
        'repeat the loop until the number of skills they can have are selected.
        Dim i As Integer = 1
        Dim chosenSkillindex As Integer
        Do
            chosenSkillindex = CInt(Int((skillOptions.Length * Rnd())))
            If skillBonuses(skillOptions(chosenSkillindex)) = 0 Then
                skillBonuses(skillOptions(chosenSkillindex)) = profBonus
                i += 1
            End If
        Loop While i <= numSkills
    End Sub

    Private Sub calculateAC()

        Dim i As Integer
        If class_ = "Monk" Then
            i = 10 + Modifiers(StatList.Dexterity) + Modifiers(StatList.Wisdom)
        Else
            i = 10 + Modifiers(StatList.Dexterity)
        End If

        Acs(0) = i & " - No Armor"
        Acs(1) = 11 + Modifiers(StatList.Dexterity) & " - Padded"
        Acs(2) = 11 + Modifiers(StatList.Dexterity) & " - Leather"
        Acs(3) = 12 + Modifiers(StatList.Dexterity) & " - Studded"

        If Modifiers(StatList.Dexterity) > 2 Then
            i = 14
        Else
            i = 12 + Modifiers(StatList.Dexterity)
        End If
        Acs(4) = i & " - Hide"

        If Modifiers(StatList.Dexterity) > 2 Then
            i = 15
        Else
            i = 13 + Modifiers(StatList.Dexterity)
        End If
        Acs(5) = i & " - Chain Shirt"

        If Modifiers(StatList.Dexterity) > 2 Then
            i = 16
        Else
            i = 14 + Modifiers(StatList.Dexterity)
        End If
        Acs(6) = i & " - Scale Mail"
        Acs(7) = i & " - Breastplate"

        If Modifiers(StatList.Dexterity) > 2 Then
            i = 17
        Else
            i = 15 + Modifiers(StatList.Dexterity)
        End If
        Acs(8) = i & " - Half Plate"

        Acs(9) = 14 & " - Ring Mail"
        Acs(10) = 16 & " - Chain Mail"
        Acs(11) = 17 & " - Splint"
        Acs(12) = 18 & " - Plate"

    End Sub


    'getters
    Public Function getRace() As String
        Return race
    End Function

    Public Function getClass() As String
        Return class_
    End Function

    Public Function getBackground() As String
        Return background
    End Function

    Public Function getSubrace() As String
        Return subrace
    End Function

    Public Function getAbilityScores() As Integer()
        Return AbilityScores
    End Function
    Public Function getAbilityScore(ByVal x As Integer) As Integer
        Return AbilityScores(x)
    End Function

    Public Function getModifiers() As Integer()
        Return Modifiers
    End Function

    Public Function getModifier(ByVal x As Integer) As Integer
        Return Modifiers(x)
    End Function

    Public Function getHP() As Integer
        Return HP
    End Function

    Public Function getHitDie() As String
        Return hitDie
    End Function

    Public Function getSpeed() As Integer
        Return speed
    End Function

    Public Function getSize() As String
        Return size
    End Function

    Public Function getProfBonus() As Integer
        Return profBonus
    End Function

    Public Function getSavingThrows() As Integer()
        Return savingThrows
    End Function

    Public Function getSavingThrow(ByVal x As Integer) As Integer
        Return savingThrows(x)
    End Function

    Public Function getSkillBonuses() As Integer()
        Return skillBonuses
    End Function

    Public Function getSkillBonus(ByVal x As Integer) As Integer
        Return skillBonuses(x)
    End Function

    Public Function getInitiative() As Integer
        Return Modifiers(StatList.Dexterity)
    End Function

    Public Function getPassivePerception() As Integer
        Return 10 + profBonus + Modifiers(StatList.Wisdom)
    End Function

    Public Function getLevel() As Integer
        Return level
    End Function

    Public Function getACs() As String()
        Return Acs
    End Function

    Public Function getAuditLog() As String
        Return auditLog
    End Function
End Class
