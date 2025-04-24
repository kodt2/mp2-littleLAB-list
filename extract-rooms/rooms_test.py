import openpyxl
import datetime
import fpdf

def numb_to_month(n):
    months = {
        1: "января", 2: "февраля", 3: "марта", 4: "апреля",
        5: "мая", 6: "июня", 7: "июля", 8: "августа",
        9: "сентября", 10: "октября", 11: "ноября", 12: "декабря"
    }
    n = int(n)
    return months.get(n, "NaM")

def time_to_pairs(start_time, end_time):
    def time_to_float(t):
        hours, minutes = map(int, t.split(":"))
        if int(hours)>24:
            raise Exception("hours should be less or equal than 24")
        if int(minutes)>60:
            raise Exception("minutes should be less or equal than 60")
        return hours + minutes / 60
    fl_start_time = time_to_float(start_time)
    fl_end_time = time_to_float(end_time)
    if (fl_start_time<7.0 or fl_end_time<7.0):
        raise Exception("Лобач открывается в 7:30")
    if (fl_start_time>22 or fl_end_time>22):
        raise Exception("Лобач закрывается в 21")
    # Время начала и конца каждой пары
    pairs = [(7.5, 7.5 + 1.5), (9.0+1/6, 9.0+1/6 + 1.5), (10.0+5/6, 10.0+5/6 + 1.5), (13.0, 13.0 + 1.5),
             (14.0+40/60, 14.0+40/60 + 1.5), (16.0+20/60, 16.0+20/60 + 1.5), (18.0, 18.0 + 1.5), (19.0+40/60, 19.0+40/60 + 1.5)]

    num_st_p = next((i + 1 for i, (start, end) in enumerate(pairs) if start <= fl_start_time <= end), 1)
    num_en_p = next((i + 1 for i, (start, end) in enumerate(pairs) if start <= fl_end_time <= end), 8)
    #print(start_time, end_time, list(range(num_st_p, num_en_p + 1)))
    return list(range(num_st_p, num_en_p + 1))


def get_aud():
    with open("outp.txt", encoding='utf-8') as f:
        inp = [line.split() for line in f.read().strip().split("\n")]

    # Используем set comprehension для создания множества и set -> sorted list
    s = sorted({int(i[6]) for i in inp})

    # Объединяем элементы списка в строку через запятую
    korp = ", ".join(map(str, s))

    return korp, inp


def nedeed_text1(pdf, korp, KrotovaNV, KrotovNV, Zolotuh_name, VolkovuAE, Borisov_name, predsed_name, predsed_phone):
    pdf.cell(200, 13, txt="", ln=1, align="R")
    pdf.cell(190, 6, txt="Верховному Хранителю Безопасности", ln=1, align="R")
    pdf.cell(190, 6, txt="Академии Высшей Магии имени Великого Архимага Лобачевского", ln=1, align="R")
    pdf.cell(190, 6, txt="Лорду В из Волчьего Ордена", ln=1, align="R")
    pdf.cell(190, 6, txt="от Верховного Наставника Заклинаний и Тайн", ln=1, align="R")
    pdf.cell(190, 6, txt="Академии Информационных Чудес и", ln=1, align="R")
    pdf.cell(190, 6, txt="Математических Волшебств", ln=1, align="R")
    pdf.cell(190, 6, txt="Мастера из Подземного Царства", ln=1, align="R")
    pdf.cell(190, 8, txt="", ln=1, align="C")
    pdf.cell(190, 15, txt="Послание через магический кристалл.", ln=1, align="C")
    pdf.cell(10)
    pdf.multi_cell(180, 6, txt="          Академия Информационных Чудес и Математических Волшебств просит"
                               "\n"
                               "Ваше Высокопочтенство предоставить доступ в"
                               " залы башни №" + korp + " Академии Высшей Магии имени Великого Архимага Лобачевского"
                                                               "для проведения собрания Совета Учеников АИЧМВ.",
               align="L")
    pdf.cell(190, 6, txt="", ln=1, align="l")
    return pdf

def nedeed_text2(pdf,KrotovaNV, KrotovNV, Zolotuh_name, VolkovuAE, Borisov_name, predsed_name, predsed_phone):
    pdf.cell(10)
    pdf.cell(190, 6, txt="Ответственные маги:", ln=1, align="l")
    pdf.cell(10)
    pdf.cell(190, 6, txt="З.Н.Ю., Б.Н.А., К.H.B.", ln=1, align="l")
    pdf.cell(190, 6, txt="", ln=1, align="l")
    pdf.cell(10)
    pdf.cell(100, 6, txt="Верховный Наставник Заклинаний и Тайн Академии", ln=1, align="l")
    pdf.cell(10)
    pdf.cell(100, 6, txt="Информационных Чудес и Математических Волшебств", align="l")
    pdf.cell(40)
    pdf.cell(90, 6, txt="Маг Подземелий К.", ln=1, align="r")
    pdf.cell(190, 6, txt="", ln=1, align="l")
    pdf.cell(190, 6, txt="Предводитель Совета Учеников АИЧМВ: Саратова Марина, говорильный камень +7573", ln=1, align="R")
    return pdf

def cycle_out_rooms(pdf, arr, start, end):
    for i in range(start, end):
        pdf.cell(10)
        pdf.cell(190, 6, txt=arr[i][0] + "-го дня месяца " + arr[i][1] + " " + arr[i][2] + u" c " + arr[i][3] + u" до " + arr[i][4]
                             + u" в зале " + arr[i][5] + u" башни № " + arr[i][6], ln=1, align="l")
    pdf.cell(190, 6 * (16 - end + start), txt="", ln=1, align="C")
    return pdf

def work_rooms(arr,KrotovaNV, KrotovNV, Zolotuh_name, VolkovuAE, Borisov_name, predsed_name, predsed_phone):
    year = datetime.date.today().year
    work2 = openpyxl.load_workbook("inp6.xlsx")
    work6 = openpyxl.load_workbook("inp2.xlsx")
    sheet2 = work2[u'Загруженность аудиторий']
    sheet6 = work6[u'Загруженность аудиторий']
    week = []
    week6 = []
    val = ''
    old_coord = int("A1"[1:])
    for cellObj in sheet2['A2':'A100']:
        for cell in cellObj:
            if cell.value != sheet2['A3'].value:
                val = cell.value.split(' ')
                new_coord = int(cell.coordinate[1:])
                week.append([val[0], val[1], val[2], val[4], old_coord, new_coord])
                old_coord = int(cell.coordinate[1:])
    #print(week)
    for i in range(len(week) - 1):
        week[i][4] = week[i + 1][4]
        week[i][5] = week[i + 1][5] - 1
    #print(week)
    week[len(week) - 1][4] = week[len(week) - 1][5]
    week[len(week) - 1][5] = week[len(week) - 1][5] + 7
    #print(week)
    val = ''
    old_coord = int("A1"[1:])
    for cellObj in sheet6['A2':'A100']:
        for cell in cellObj:
            if cell.value != sheet6['A3'].value:
                val = cell.value.split(' ')
                new_coord = int(cell.coordinate[1:])
                week6.append([val[0], val[1], val[2], val[4], old_coord, new_coord])
                old_coord = int(cell.coordinate[1:])
    for i in range(len(week6) - 1):
        week6[i][4] = week6[i + 1][4]
        week6[i][5] = week6[i + 1][5] - 1
    week6[len(week6) - 1][4] = week6[len(week6) - 1][5]
    week6[len(week6) - 1][5] = week6[len(week6) - 1][5] + 7
    for i in week:
        room_data = []
        for cellObj in sheet2['W' + str(i[4]):'AE' + str(i[5])]:
            help = []
            for cell in cellObj:
                if (cell.value == None):
                    help.append('')
                else:
                    help.append(cell.value)
            room_data.append(help)
        i.append(room_data)
        while len(i[6]) <= 7:
            i[6].append(['', '', '', '', '', '', '', '', ''])
    #print(week6)
    for i in week6:
        room_data = []
        for cellObj in sheet6['C' + str(i[4]):'AT' + str(i[5])]:
            help = []
            for cell in cellObj:
                if (cell.value == None):
                    help.append('')
                else:
                    help.append(cell.value)
            room_data.append(help)
        i.append(room_data)
        while len(i[6]) <= 7:
            i[6].append(
                ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '',
                 '',
                 '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''])
    # print("###########################################")
    #print(week6)
    req = []
    for i in arr:
        try:
            month = numb_to_month((i[3].split('.'))[1])
        except Exception("wrong date format Expected: [date.month]"):
            print("wrong date format Expected: [date.month]")
        try:
            req.append([i[0]+" "+ i[1], i[2], str(int(i[3].split('.')[0])), month, year, time_to_pairs(i[4], i[5]), i[6], i[4], i[5]])
        except Exception("wrong time format Expected hours:minutes"):
            print("wrong time format Expected")

    vihod = []
    #print(req)
    for i in req:
        #print(i)
        if i[6] == 'big':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            for j in week:
                if (day == j[0] and month == j[1] and year == int(j[2])):
                    cvobodn = 1
                    right_week = 1
                    for p in i[5]:
                        if (j[6][p - 1][6] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][6] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 513, 6])
                        also_find = 1
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][0] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][0] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 502, 6])
                        also_find = 1
                        break
            if (also_find == 0):
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        cvobodn = 1
                        right_week = 1
                        for p in i[5]:
                            if (j[6][p - 1][0] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][0] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 105, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][36] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][36] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 314, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][43] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][43] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 328, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][32] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][32] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 307, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][42] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][42] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 324, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][33] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][33] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 309, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == 'big2':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            for j in week6:
                if (day == j[0] and month == j[1] and year == int(j[2])):
                    cvobodn = 1
                    right_week = 1
                    for p in i[5]:
                        if (j[6][p - 1][0] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][0] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 105, 2])
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][36] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][36] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 314, 2])
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][43] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][43] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 328, 2])
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][32] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][32] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 307, 2])
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][42] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][42] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 324, 2])
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][33] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][33] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 309, 2])
                        break
                    if cvobodn == 0:
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '105':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][0] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][0] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 105, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '307':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][32] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][32] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 307, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '309':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][33] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][33] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 309, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '314':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][36] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][36] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 314, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '317':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][37] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][37] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 317, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '318':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][39] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][39] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 318, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '322':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][41] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][41] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 322, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '324':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][42] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][42] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 324, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '328':
            #print(i)
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            for j in week6:
                if (day == j[0] and month == j[1] and year == int(j[2])):
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][43] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][43] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 328, 2])
                        break
                    if cvobodn == 0:
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])

        elif i[6] == 'any':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            for j in week:
                if (day == j[0] and month == j[1] and year == int(j[2])):
                    right_week = 1
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][0] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][0] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 502, 6])
                        also_find = 1
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][1] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][1] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 506, 6])
                        also_find = 1
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][2] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][2] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 508, 6])
                        also_find = 1
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][3] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][3] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 509, 6])
                        also_find = 1
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][4] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][4] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 511, 6])
                        also_find = 1
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][5] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][5] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 512, 6])
                        also_find = 1
                        break
                    for p in i[5]:
                        if (j[6][p - 1][6] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][6] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 513, 6])
                        also_find = 1
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][7] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][7] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 514, 6])
                        also_find = 1
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][8] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][8] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 515, 6])
                        also_find = 1
                        break
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][0] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][0] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 105, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][32] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][32] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 307, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][33] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][33] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 309, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][36] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][36] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 314, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][37] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][37] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 317, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][39] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][39] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 318, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][41] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][41] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 322, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][42] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][42] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 324, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][43] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][43] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 328, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == 'any2':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][0] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][0] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 105, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][32] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][32] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 307, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][33] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][33] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 309, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][36] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][36] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 314, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][37] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][37] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 317, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][39] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][39] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 318, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][41] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][41] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 322, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][42] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][42] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 324, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][43] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][43] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 328, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        else:
            vihod.append([i[0] + " " + i[1], int(i[2]), i[3], i[4], i[7], i[8], "", "wrong postfix "+"'"+i[6]+"'"])
    print(vihod)
    return(vihod)
    #vihod.sort(key=lambda x: x[2])
    #vihod.sort(key=lambda x: x[1])
    #with open('outp.txt', 'w', encoding='utf-8') as f:
    #    for i in range(len(vihod) - 1):
    #        for j in range(1, len(vihod[i]) - 1):
    #            f.write(str(vihod[i][j]) + " ")
    #        f.write(str(vihod[i][j + 1]) + "\n")
    #    for j in range(1, len(vihod[len(vihod) - 1]) - 1):
    #        f.write(str(vihod[len(vihod) - 1][j]) + " ")
    #    f.write(str(vihod[len(vihod) - 1][j + 1]))
    #vihod.sort(key=lambda x: x[0])
    #print(vihod)


def work_pdf(arr,KrotovaNV, KrotovNV, Zolotuh_name, VolkovuAE, Borisov_name, predsed_name, predsed_phone):
    year = datetime.date.today().year
    work2 = openpyxl.load_workbook("inp6.xlsx")
    work6 = openpyxl.load_workbook("inp2.xlsx")
    sheet2 = work2[u'Загруженность аудиторий']
    sheet6 = work6[u'Загруженность аудиторий']
    week = []
    week6 = []
    val = ''
    old_coord = int("A1"[1:])
    for cellObj in sheet2['A2':'A100']:
        for cell in cellObj:
            if cell.value != sheet2['A3'].value:
                val = cell.value.split(' ')
                new_coord = int(cell.coordinate[1:])
                week.append([val[0], val[1], val[2], val[4], old_coord, new_coord])
                old_coord = int(cell.coordinate[1:])
    #print(week)
    for i in range(len(week) - 1):
        week[i][4] = week[i + 1][4]
        week[i][5] = week[i + 1][5] - 1
    #print(week)
    week[len(week) - 1][4] = week[len(week) - 1][5]
    week[len(week) - 1][5] = week[len(week) - 1][5] + 7
    #print(week)
    val = ''
    old_coord = int("A1"[1:])
    for cellObj in sheet6['A2':'A100']:
        for cell in cellObj:
            if cell.value != sheet6['A3'].value:
                val = cell.value.split(' ')
                new_coord = int(cell.coordinate[1:])
                week6.append([val[0], val[1], val[2], val[4], old_coord, new_coord])
                old_coord = int(cell.coordinate[1:])
    for i in range(len(week6) - 1):
        week6[i][4] = week6[i + 1][4]
        week6[i][5] = week6[i + 1][5] - 1
    week6[len(week6) - 1][4] = week6[len(week6) - 1][5]
    week6[len(week6) - 1][5] = week6[len(week6) - 1][5] + 7
    for i in week:
        room_data = []
        for cellObj in sheet2['W' + str(i[4]):'AE' + str(i[5])]:
            help = []
            for cell in cellObj:
                if (cell.value == None):
                    help.append('')
                else:
                    help.append(cell.value)
            room_data.append(help)
        i.append(room_data)
        while len(i[6]) <= 7:
            i[6].append(['', '', '', '', '', '', '', '', ''])
    #print(week6)
    for i in week6:
        room_data = []
        for cellObj in sheet6['C' + str(i[4]):'AT' + str(i[5])]:
            help = []
            for cell in cellObj:
                if (cell.value == None):
                    help.append('')
                else:
                    help.append(cell.value)
            room_data.append(help)
        i.append(room_data)
        while len(i[6]) <= 7:
            i[6].append(
                ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '',
                 '',
                 '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''])
    # print("###########################################")
    #print(week6)
    req = []
    for i in arr:
        try:
            month = numb_to_month((i[3].split('.'))[1])
        except Exception("wrong date format Expected: [date.month]"):
            print("wrong date format Expected: [date.month]")
        try:
            req.append([i[0]+" "+ i[1], i[2], str(int(i[3].split('.')[0])), month, year, time_to_pairs(i[4], i[5]), i[6], i[4], i[5]])
        except Exception("wrong time format Expected hours:minutes"):
            print("wrong time format Expected")

    vihod = []
    print(req)
    for i in req:
        #print(i)
        if i[6] == 'big':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            for j in week:
                if (day == j[0] and month == j[1] and year == int(j[2])):
                    cvobodn = 1
                    right_week = 1
                    for p in i[5]:
                        if (j[6][p - 1][6] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][6] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 513, 6])
                        also_find = 1
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][0] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][0] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 502, 6])
                        also_find = 1
                        break
            if (also_find == 0):
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        cvobodn = 1
                        right_week = 1
                        for p in i[5]:
                            if (j[6][p - 1][0] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][0] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 105, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][36] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][36] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 314, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][43] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][43] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 328, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][32] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][32] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 307, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][42] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][42] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 324, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][33] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][33] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 309, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == 'big2':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            for j in week6:
                if (day == j[0] and month == j[1] and year == int(j[2])):
                    cvobodn = 1
                    right_week = 1
                    for p in i[5]:
                        if (j[6][p - 1][0] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][0] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 105, 2])
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][36] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][36] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 314, 2])
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][43] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][43] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 328, 2])
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][32] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][32] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 307, 2])
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][42] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][42] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 324, 2])
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][33] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][33] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 309, 2])
                        break
                    if cvobodn == 0:
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '105':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][0] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][0] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 105, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '307':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][32] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][32] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 307, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '309':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][33] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][33] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 309, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '314':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][36] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][36] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 314, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '317':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][37] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][37] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 317, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '318':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][39] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][39] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 318, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '322':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][41] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][41] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 322, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '324':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][42] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][42] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 324, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == '328':
            #print(i)
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            for j in week6:
                if (day == j[0] and month == j[1] and year == int(j[2])):
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][43] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][43] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 328, 2])
                        break
                    if cvobodn == 0:
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])

        elif i[6] == 'any':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            for j in week:
                if (day == j[0] and month == j[1] and year == int(j[2])):
                    right_week = 1
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][0] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][0] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 502, 6])
                        also_find = 1
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][1] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][1] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 506, 6])
                        also_find = 1
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][2] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][2] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 508, 6])
                        also_find = 1
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][3] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][3] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 509, 6])
                        also_find = 1
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][4] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][4] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 511, 6])
                        also_find = 1
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][5] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][5] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 512, 6])
                        also_find = 1
                        break
                    for p in i[5]:
                        if (j[6][p - 1][6] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][6] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 513, 6])
                        also_find = 1
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][7] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][7] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 514, 6])
                        also_find = 1
                        break
                    cvobodn = 1
                    for p in i[5]:
                        if (j[6][p - 1][8] != ''):
                            cvobodn = 0
                    if cvobodn == 1:
                        for p in i[5]:
                            j[6][p - 1][8] = 'CCC'
                        vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 515, 6])
                        also_find = 1
                        break
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][0] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][0] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 105, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][32] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][32] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 307, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][33] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][33] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 309, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][36] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][36] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 314, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][37] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][37] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 317, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][39] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][39] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 318, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][41] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][41] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 322, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][42] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][42] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 324, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][43] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][43] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 328, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        elif i[6] == 'any2':
            day = i[2]
            month = i[3]
            year = i[4]
            pairs = i[5]
            also_find = 0
            right_week = 0
            if also_find == 0:
                for j in week6:
                    if (day == j[0] and month == j[1] and year == int(j[2])):
                        right_week = 1
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][0] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][0] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 105, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][32] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][32] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 307, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][33] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][33] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 309, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][36] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][36] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 314, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][37] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][37] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 317, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][39] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][39] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 318, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][41] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][41] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 322, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][42] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][42] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 324, 2])
                            break
                        cvobodn = 1
                        for p in i[5]:
                            if (j[6][p - 1][43] != ''):
                                cvobodn = 0
                        if cvobodn == 1:
                            for p in i[5]:
                                j[6][p - 1][43] = 'CCC'
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], 328, 2])
                            break
                        if cvobodn == 0:
                            vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no free room"])
            if (right_week == 0):
                vihod.append([i[0] + " " + i[1], int(day), month, year, i[7], i[8], "", "no such day"])
        else:
            vihod.append([i[0] + " " + i[1], int(i[2]), i[3], i[4], i[7], i[8], "", "wrong postfix "+"'"+i[6]+"'"])
    #print(vihod)
    vihod.sort(key=lambda x: x[2])
    vihod.sort(key=lambda x: x[1])
    with open('outp.txt', 'w', encoding='utf-8') as f:
        for i in range(len(vihod) - 1):
            for j in range(1, len(vihod[i]) - 1):
                f.write(str(vihod[i][j]) + " ")
            f.write(str(vihod[i][j + 1]) + "\n")
        for j in range(1, len(vihod[len(vihod) - 1]) - 1):
            f.write(str(vihod[len(vihod) - 1][j]) + " ")
        f.write(str(vihod[len(vihod) - 1][j + 1]))
    vihod.sort(key=lambda x: x[0])
    #print(vihod)
    with open('Message.txt', 'w', encoding='utf-8') as f:
        for i in vihod:
            for j in range(0, len(i) - 1):
                f.write(str(i[j]) + " ")
            f.write(str(i[j + 1]) + "\n")
    arr = list()
    korp =''
    try:
        korp, arr = get_aud()
    except Exception:
        print("Exept: no such room")
        raise Exception("no such room")
    pdf = fpdf.FPDF(format='letter')  # pdf format
    pdf.add_page()
    pdf.add_font('Times', '', 'times.ttf', uni=True)
    pdf.set_font("Times", size=14)
    pdf = nedeed_text1(pdf, korp,KrotovaNV, KrotovNV, Zolotuh_name, VolkovuAE, Borisov_name, predsed_name, predsed_phone)

    if len(arr) <= 14:
        pdf = cycle_out_rooms(pdf, arr, 0, len(arr))
        pdf = nedeed_text2(pdf,KrotovaNV, KrotovNV, Zolotuh_name, VolkovuAE, Borisov_name, predsed_name, predsed_phone)
    else:
        count_pages = len(arr) // 14
        if (len(arr) % 13 != 0):
            count_pages += 1
        pdf = cycle_out_rooms(pdf, arr, 0, 14)
        pdf = nedeed_text2(pdf,KrotovaNV, KrotovNV, Zolotuh_name, VolkovuAE, Borisov_name, predsed_name, predsed_phone)

        for i in range(1, count_pages - 1):
            pdf.add_page()
            pdf = nedeed_text1(pdf,korp,KrotovaNV, KrotovNV, Zolotuh_name, VolkovuAE, Borisov_name, predsed_name, predsed_phone)
            pdf = cycle_out_rooms(pdf, arr, i * 14, i * 14 + 14)
            pdf = nedeed_text2(pdf,KrotovaNV, KrotovNV, Zolotuh_name, VolkovuAE, Borisov_name, predsed_name, predsed_phone)
        pdf.add_page()
        pdf = nedeed_text1(pdf,korp,KrotovaNV, KrotovNV, Zolotuh_name, VolkovuAE, Borisov_name, predsed_name, predsed_phone)
        pdf = cycle_out_rooms(pdf, arr, (count_pages - 1) * 14, len(arr))
        pdf = nedeed_text2(pdf,KrotovaNV, KrotovNV, Zolotuh_name, VolkovuAE, Borisov_name, predsed_name, predsed_phone)

    pdf.output("test.pdf")
    return vihod

'''with open('needed rooms.txt', encoding='utf-8') as f:
    arr = f.read()
    arr = arr.split('\n')
    for i in range(len(arr)):
        arr[i] = arr[i].split(' ')

work_rooms(arr)'''