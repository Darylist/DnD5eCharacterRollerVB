Public Class Form1
    Private cha As Character
    Private savefile As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        loadCombos()

    End Sub

    Private Sub btnRoll_Click(sender As Object, e As EventArgs) Handles btnRoll.Click
        cha = New Character()
        displayCharacter()
        btnSave.Enabled = True
        btnReRoll.Enabled = True
    End Sub

    Private Sub displayCharacter()
        cboRace.SelectedItem = cha.getRace
        cboClass.SelectedItem = cha.getClass
        cboBackground.SelectedItem = cha.getBackground
        cboSubRace.Items.Clear()
        cboSubRace.Items.Add("Random")
        cboSubRace.Items.AddRange(Character.getSubraces(cboRace.SelectedIndex))
        cboSubRace.SelectedItem = cha.getSubrace
        txtStr.Text = cha.getAbilityScore(Character.StatList.Strength).ToString
        txtDex.Text = cha.getAbilityScore(Character.StatList.Dexterity).ToString
        txtCon.Text = cha.getAbilityScore(Character.StatList.Constitution).ToString
        txtInt.Text = cha.getAbilityScore(Character.StatList.Intelligence).ToString
        txtWis.Text = cha.getAbilityScore(Character.StatList.Wisdom).ToString
        txtCha.Text = cha.getAbilityScore(Character.StatList.Charisma).ToString

        txtStrM.Text = cha.getModifier(Character.StatList.Strength).ToString
        txtDexM.Text = cha.getModifier(Character.StatList.Dexterity).ToString
        txtConM.Text = cha.getModifier(Character.StatList.Constitution).ToString
        txtIntM.Text = cha.getModifier(Character.StatList.Intelligence).ToString
        txtWisM.Text = cha.getModifier(Character.StatList.Wisdom).ToString
        txtChaM.Text = cha.getModifier(Character.StatList.Charisma).ToString

        txtHP.Text = cha.getHP
        txtHitDie.Text = cha.getHitDie
        txtProf.Text = cha.getProfBonus
        txtSpeed.Text = cha.getSpeed
        txtSize.Text = cha.getSize

        txtStrST.Text = cha.getSavingThrow(Character.StatList.Strength).ToString
        txtDexST.Text = cha.getSavingThrow(Character.StatList.Dexterity).ToString
        txtConST.Text = cha.getSavingThrow(Character.StatList.Constitution).ToString
        txtIntST.Text = cha.getSavingThrow(Character.StatList.Intelligence).ToString
        txtWisST.Text = cha.getSavingThrow(Character.StatList.Wisdom).ToString
        txtChaST.Text = cha.getSavingThrow(Character.StatList.Charisma).ToString

        txtAcrobatics.Text = cha.getSkillBonus(Character.SkillList.ACROBATICS).ToString
        txtAnimalHandling.Text = cha.getSkillBonus(Character.SkillList.ANIMAL_HANDLING).ToString
        txtArcana.Text = cha.getSkillBonus(Character.SkillList.ARCANA).ToString
        txtAthletics.Text = cha.getSkillBonus(Character.SkillList.ATHLETICS).ToString
        txtDeception.Text = cha.getSkillBonus(Character.SkillList.DECEPTION).ToString
        txtHistory.Text = cha.getSkillBonus(Character.SkillList.HISTORY).ToString
        txtInsight.Text = cha.getSkillBonus(Character.SkillList.INSIGHT).ToString
        txtIntimidation.Text = cha.getSkillBonus(Character.SkillList.INTIMIDATION).ToString
        txtInvestigation.Text = cha.getSkillBonus(Character.SkillList.INVESTIGATION).ToString
        txtMedicine.Text = cha.getSkillBonus(Character.SkillList.MEDICINE).ToString
        txtNature.Text = cha.getSkillBonus(Character.SkillList.NATURE).ToString
        txtPerception.Text = cha.getSkillBonus(Character.SkillList.PERCEPTION).ToString
        txtPerformance.Text = cha.getSkillBonus(Character.SkillList.PERFORMANCE).ToString
        txtPersuasion.Text = cha.getSkillBonus(Character.SkillList.PERSUASION).ToString
        txtReligion.Text = cha.getSkillBonus(Character.SkillList.RELIGION).ToString
        txtSleightofHand.Text = cha.getSkillBonus(Character.SkillList.SLEIGHT_OF_HAND).ToString
        txtStealth.Text = cha.getSkillBonus(Character.SkillList.STEALTH).ToString
        txtSurvival.Text = cha.getSkillBonus(Character.SkillList.SURVIVAL).ToString

        txtInit.Text = cha.getInitiative.ToString
        txtPP.Text = cha.getPassivePerception.ToString
        txtLevel.Text = cha.getLevel
        cboAC.Items.Clear()
        cboAC.Items.AddRange(cha.getACs)
        cboAlignment.SelectedIndex = 0
        cboAC.SelectedIndex = 0
        txtAuditLog.Text = cha.getAuditLog
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Dim subracetext As String = ""
        If cha.getSubrace <> "N/A" Then
            subracetext = " (" & cha.getSubrace & ")"
        End If
        savefile = "%FDF-1.2
1 0 obj
<</FDF<</F(Character Sheet - Form Fillable.pdf)/UF(Character Sheet - Form Fillable.pdf)/Fields[<</T<436C6173734C6576656C>/V(" & cha.getClass & " Level 1" & ")>><</T<4261636B67726F756E64>/V(" & cha.getBackground & ")>><</T<506C617965724E616D65>>><</T<4368617261637465724E616D65>/V(" & txtName.Text & ")>><</T<5261636520>/V(" & cha.getRace & subracetext & ")>><</T<416C69676E6D656E74>/V(" & cboAlignment.SelectedItem.ToString & ")>><</T<5850>>><</T<496E737069726174696F6E>>><</T(STR)/V(" & cha.getAbilityScore(Character.StatList.Strength) & ")>><</T<50726F66426F6E7573>/V(" & cha.getProfBonus & ")>><</T(AC)/V(" & cboAC.SelectedItem.ToString.Substring(0, 2) & ")>><</T<496E6974696174697665>/V(" & cha.getInitiative & ")>><</T<5370656564>/V(" & cha.getSpeed & ")>><</T(PersonalityTraits )>><</T<5354526D6F64>/V(" & cha.getModifier(Character.StatList.Strength) & ")>><</T<48504D6178>/V(" & cha.getHP & ")>><</T<535420537472656E677468>/V(" & cha.getSavingThrow(Character.StatList.Strength) & ")>><</T(DEX)/V(" & cha.getAbilityScore(Character.StatList.Dexterity) & ")>><</T<485043757272656E74>>><</T(Ideals)>><</T(DEXmod )/V(" & cha.getModifier(Character.StatList.Dexterity) & ")>><</T<485054656D70>>><</T<426F6E6473>>><</T<434F4E>/V(" & cha.getAbilityScore(Character.StatList.Constitution) & ")>><</T<4844546F74616C>/V(" & cha.getHitDie & ")>><</T(Check Box 12)/V/Off>><</T<436865636B20426F78203133>/V/Off>><</T<436865636B20426F78203134>/V/Off>><</T<434F4E6D6F64>/V(" & cha.getModifier(Character.StatList.Constitution) & ")>><</T<436865636B20426F78203135>/V/Off>><</T<436865636B20426F78203136>/V/Off>><</T<436865636B20426F78203137>/V/Off>><</T<4844>>><</T<466C617773>>><</T<494E54>/V(" & cha.getAbilityScore(Character.StatList.Intelligence) & ")>><</T<535420446578746572697479>/V(" & cha.getSavingThrow(Character.StatList.Dexterity) & ")>><</T<535420436F6E737469747574696F6E>/V(" & cha.getSavingThrow(Character.StatList.Constitution) & ")>><</T<535420496E74656C6C6967656E6365>/V(" & cha.getSavingThrow(Character.StatList.Intelligence) & ")>><</T(ST Wisdom)/V(" & cha.getSavingThrow(Character.StatList.Wisdom) & ")>><</T<5354204368617269736D61>/V(" & cha.getSavingThrow(Character.StatList.Charisma) & ")>><</T(Acrobatics)/V(" & cha.getSkillBonus(Character.SkillList.ACROBATICS) & ")>><</T(Animal)/V(" & cha.getSkillBonus(Character.SkillList.ANIMAL_HANDLING) & ")>><</T<4174686C6574696373>/V(" & cha.getSkillBonus(Character.SkillList.ATHLETICS) & ")>><</T<446563657074696F6E20>/V(" & cha.getSkillBonus(Character.SkillList.DECEPTION) & ")>><</T<486973746F727920>/V(" & cha.getSkillBonus(Character.SkillList.HISTORY) & ")>><</T<496E7369676874>/V(" & cha.getSkillBonus(Character.SkillList.INSIGHT) & ")>><</T(Intimidation)/V(" & cha.getSkillBonus(Character.SkillList.INTIMIDATION) & ")>><</T<436865636B20426F78203131>/V/Off>><</T<436865636B20426F78203138>/V/Off>><</T<436865636B20426F78203139>/V/Off>><</T<436865636B20426F78203230>/V/Off>><</T<436865636B20426F78203231>/V/Off>><</T<436865636B20426F78203232>/V/Off>><</T<57706E204E616D65>>><</T<57706E312041746B426F6E7573>>><</T<57706E312044616D616765>>><</T<494E546D6F64>/V(" & cha.getModifier(Character.StatList.Intelligence) & ")>><</T<57706E204E616D652032>>><</T<57706E322041746B426F6E757320>>><</T<57706E322044616D61676520>>><</T<496E7665737469676174696F6E20>/V(" & cha.getSkillBonus(Character.SkillList.INVESTIGATION) & ")>><</T<574953>/V(" & cha.getAbilityScore(Character.StatList.Wisdom) & ")>><</T<57706E204E616D652033>>><</T<57706E332041746B426F6E75732020>>><</T<417263616E61>/V(" & cha.getSkillBonus(Character.SkillList.ARCANA) & ")>><</T<57706E332044616D61676520>>><</T<50657263657074696F6E20>/V(" & cha.getSkillBonus(Character.SkillList.PERCEPTION) & ")>><</T<5749536D6F64>/V(" & cha.getModifier(Character.StatList.Wisdom) & ")>><</T<434841>/V(" & cha.getAbilityScore(Character.StatList.Charisma) & ")>><</T<4E6174757265>/V(" & cha.getSkillBonus(Character.SkillList.NATURE) & ")>><</T<506572666F726D616E6365>/V(" & cha.getSkillBonus(Character.SkillList.PERFORMANCE) & ")>><</T<4D65646963696E65>/V(" & cha.getSkillBonus(Character.SkillList.MEDICINE) & ")>><</T<52656C6967696F6E>/V(" & cha.getSkillBonus(Character.SkillList.RELIGION) & ")>><</T<537465616C746820>/V(" & cha.getSkillBonus(Character.SkillList.STEALTH) & ")>><</T<436865636B20426F78203233>/V/Off>><</T<436865636B20426F78203234>/V/Off>><</T<436865636B20426F78203235>/V/Off>><</T<436865636B20426F78203236>/V/Off>><</T<436865636B20426F78203237>/V/Off>><</T<436865636B20426F78203238>/V/Off>><</T<436865636B20426F78203239>/V/Off>><</T<436865636B20426F78203330>/V/Off>><</T<436865636B20426F78203331>/V/Off>><</T<436865636B20426F78203332>/V/Off>><</T<436865636B20426F78203333>/V/Off>><</T<436865636B20426F78203334>/V/Off>><</T<436865636B20426F78203335>/V/Off>><</T<436865636B20426F78203336>/V/Off>><</T<436865636B20426F78203337>/V/Off>><</T<436865636B20426F78203338>/V/Off>><</T<436865636B20426F78203339>/V/Off>><</T<436865636B20426F78203430>/V/Off>><</T<50657273756173696F6E>/V(" & cha.getSkillBonus(Character.SkillList.PERSUASION) & ")>><</T<536C65696768746F6648616E64>/V(" & cha.getSkillBonus(Character.SkillList.SLEIGHT_OF_HAND) & ")>><</T<4348616D6F64>/V(" & cha.getModifier(Character.StatList.Charisma) & ")>><</T<537572766976616C>/V(" & cha.getSkillBonus(Character.SkillList.SURVIVAL) & ")>><</T<41747461636B735370656C6C63617374696E67>>><</T<50617373697665>/V(" & cha.getPassivePerception & ")>><</T<4350>>><</T<50726F66696369656E636965734C616E67>>><</T<5350>>><</T<4550>>><</T<4750>>><</T<5050>>><</T<45717569706D656E74>>><</T<466561747572657320616E6420547261697473>>>]>>>>
endobj

trailer
<</Root 1 0 R>>
%%EOF
"
        SaveFileDialog1.ShowDialog()
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        My.Computer.FileSystem.WriteAllText(SaveFileDialog1.FileName, savefile, False)
    End Sub

    Private Sub btnReRoll_Click(sender As Object, e As EventArgs) Handles btnReRoll.Click
        cha.rerollCharacter(cboRace.SelectedItem.ToString, cboClass.SelectedItem.ToString, cboBackground.SelectedItem.ToString, cboSubRace.SelectedItem.ToString)
        displayCharacter()
        btnSave.Enabled = True
    End Sub

    Private Sub cboClass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboClass.SelectedIndexChanged
        clearGeneratedFields()
    End Sub

    Private Sub clearGeneratedFields()

        txtStr.Text = ""
        txtDex.Text = ""
        txtCon.Text = ""
        txtInt.Text = ""
        txtWis.Text = ""
        txtCha.Text = ""

        txtStrM.Text = ""
        txtDexM.Text = ""
        txtConM.Text = ""
        txtIntM.Text = ""
        txtWisM.Text = ""
        txtChaM.Text = ""

        txtHP.Text = ""
        txtHitDie.Text = ""
        txtProf.Text = ""
        txtSpeed.Text = ""
        txtSize.Text = ""

        txtStrST.Text = ""
        txtDexST.Text = ""
        txtConST.Text = ""
        txtIntST.Text = ""
        txtWisST.Text = ""
        txtChaST.Text = ""

        txtAcrobatics.Text = ""
        txtAnimalHandling.Text = ""
        txtArcana.Text = ""
        txtAthletics.Text = ""
        txtDeception.Text = ""
        txtHistory.Text = ""
        txtInsight.Text = ""
        txtIntimidation.Text = ""
        txtInvestigation.Text = ""
        txtMedicine.Text = ""
        txtNature.Text = ""
        txtPerception.Text = ""
        txtPerformance.Text = ""
        txtPersuasion.Text = ""
        txtReligion.Text = ""
        txtSleightofHand.Text = ""
        txtStealth.Text = ""
        txtSurvival.Text = ""

        txtInit.Text = ""
        txtPP.Text = ""
        txtLevel.Text = ""
        cboAC.Items.Clear()

        cboAlignment.SelectedIndex = 0
        cboAC.Text = ""
        txtAuditLog.Text = ""
        btnSave.Enabled = False
    End Sub

    Private Sub cboRace_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRace.SelectedIndexChanged
        clearGeneratedFields()
        cboSubRace.Items.Clear()
        cboSubRace.Items.Add("Random")
        cboSubRace.Items.AddRange(Character.getSubraces(cboRace.SelectedIndex))
        cboSubRace.SelectedIndex = 0
    End Sub

    Private Sub cboBackground_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBackground.SelectedIndexChanged
        clearGeneratedFields()
    End Sub

    Private Sub cboSubRace_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSubRace.SelectedIndexChanged
        clearGeneratedFields()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        clearGeneratedFields()
        cboSubRace.Items.Clear()
        cboSubRace.Items.AddRange(Character.getSubraces(cboRace.SelectedIndex))
        cboSubRace.Text = ""
        cboRace.Items.Clear()
        cboRace.Text = ""
        cboClass.Items.Clear()
        cboClass.Text = ""
        cboBackground.Items.Clear()
        cboBackground.Text = ""
        cboAlignment.Items.Clear()
        cboAlignment.Text = ""
        btnReRoll.Enabled = False
        loadCombos()
    End Sub

    Private Sub loadCombos()
        cboAlignment.Items.AddRange(Character.GetAlignments)
        cboClass.Items.AddRange(Character.GetClasses)
        cboRace.Items.AddRange(Character.GetRaces)
        cboBackground.Items.AddRange(Character.GetBackgrounds)
    End Sub
End Class
