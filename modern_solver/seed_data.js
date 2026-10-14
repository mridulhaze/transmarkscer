// Seed dataset for NU H-Type Solver Modern Web Platform
// Extracted directly from solving_database.sql

window.NU_SEED_DATA = {
  // All 23 solvers from dbo.solvers table in solving_database.sql
  solvers: [
    { username: "giash", passkey: "1234", fullName: "Gias Sir", role: "solver", status: "Active" },
    { username: "rakib", passkey: "1234", fullName: "Rakib", role: "solver", status: "Active" },
    { username: "amin", passkey: "123", fullName: "Aminul Islam", role: "solver", status: "Active" },
    { username: "arshed", passkey: "841433", fullName: "Arshed", role: "solver", status: "Active" },
    { username: "biswas", passkey: "p123", fullName: "Prodip Kumar Biswas", role: "solver", status: "Active" },
    { username: "esrafil", passkey: "esrafil", fullName: "H M Israfil", role: "solver", status: "Active" },
    { username: "faruk", passkey: "comilla", fullName: "Dr. Faruk", role: "solver", status: "Active" },
    { username: "jakir", passkey: "32044", fullName: "Md. Jakir Mukswd", role: "solver", status: "Active" },
    { username: "kafil", passkey: "cof", fullName: "Kafil Uddin Mahmud Mojumder", role: "solver", status: "Active" },
    { username: "kazal", passkey: "465481km", fullName: "Kazal Kumar Roy", role: "solver", status: "Active" },
    { username: "mgias", passkey: "123", fullName: "Md. Giyas Uddin", role: "solver", status: "Active" },
    { username: "mostafa", passkey: "dulal", fullName: "Mohammad Mostafa", role: "solver", status: "Active" },
    { username: "Najmul", passkey: "3325", fullName: "Najmul Huda", role: "solver", status: "Active" },
    { username: "nasir", passkey: "841432", fullName: "Nasir Mia", role: "solver", status: "Active" },
    { username: "pbarai", passkey: "Pbarai", fullName: "Prodip Barai", role: "solver", status: "Active" },
    { username: "pro", passkey: "456", fullName: "Proholad Roy", role: "solver", status: "Active" },
    { username: "sazzad", passkey: "`12", fullName: "Sazzad", role: "solver", status: "Active" },
    { username: "shamim", passkey: "123", fullName: "Shamim", role: "solver", status: "Active" },
    { username: "shipra", passkey: "rani", fullName: "Shipra Rani Das", role: "solver", status: "Active" },
    { username: "m", passkey: "m", fullName: "Mr. M", role: "solver", status: "Active" },
    { username: "a", passkey: "a", fullName: "Mridul Roy", role: "uploader", status: "Admin" },
    { username: "Sweety", passkey: "Sweety", fullName: "Sweety Rani Das", role: "solver", status: "Active" }
  ],

  files: [
    {
      fileName: "B01111H1A.txt",
      paperCode: "B01111",
      solverName: "giash",
      status: "SOLVED",
      totalRows: 120,
      correctedRows: 18,
      startDate: "2026-08-03",
      endDate: "2026-08-03"
    },
    {
      fileName: "B01111H1B.txt",
      paperCode: "B01111",
      solverName: "giash",
      status: "SOLVED",
      totalRows: 95,
      correctedRows: 12,
      startDate: "2026-08-03",
      endDate: "2026-08-03"
    },
    {
      fileName: "B0139H1A.txt",
      paperCode: "B0139H",
      solverName: "rakib",
      status: "SOLVING",
      totalRows: 150,
      correctedRows: 42,
      startDate: "2026-08-08",
      endDate: null
    },
    {
      fileName: "B0141H1A.txt",
      paperCode: "B0141H",
      solverName: "amin",
      status: "PENDING",
      totalRows: 210,
      correctedRows: 0,
      startDate: null,
      endDate: null
    },
    {
      fileName: "B0143H1A.txt",
      paperCode: "B0143H",
      solverName: "faruk",
      status: "PENDING",
      totalRows: 88,
      correctedRows: 0,
      startDate: null,
      endDate: null
    }
  ],

  validPaperCodes: [
    "B01111", "B0139H", "B0141H", "B0143H", "B0145H", "B0147H",
    "810301", "810303", "810305", "810307", "000320", "000322",
    "510101", "510103", "221101", "221103", "221105"
  ],

  sampleRows: {
    "B0139H1A.txt": [
      {
        id: 101,
        serial: 1,
        pap_code: "B0139H",
        eb_no: "50123",
        sl_no: "001",
        marks: "075",
        hmarks: "075",
        bdexcode: "11110000101010101010101000100010",
        bhexcode: "11110000101010101010101000100010",
        err_dscr: "",
        comments: "Verified OK"
      },
      {
        id: 102,
        serial: 2,
        pap_code: "B0139X",
        eb_no: "50124",
        sl_no: "002",
        marks: "068",
        hmarks: "068",
        bdexcode: "11110000110011001100110000100010",
        bhexcode: "11110000110011001100110000100010",
        err_dscr: "Sub. Invalid",
        comments: "Paper code typo"
      },
      {
        id: 103,
        serial: 3,
        pap_code: "B0139H",
        eb_no: "501",
        sl_no: "003",
        marks: "082",
        hmarks: "082",
        bdexcode: "11110011001100110011001100100010",
        bhexcode: "11110011001100110011001100100010",
        err_dscr: "EB. Len.",
        comments: "Missing examiner leading zeros"
      },
      {
        id: 104,
        serial: 4,
        pap_code: "B0139H",
        eb_no: "50126",
        sl_no: "04",
        marks: "045",
        hmarks: "045",
        bdexcode: "11110101010101010101010100100010",
        bhexcode: "11110101010101010101010100100010",
        err_dscr: "SL. Len.",
        comments: "Serial length 2 digits"
      },
      {
        id: 105,
        serial: 5,
        pap_code: "B0139H",
        eb_no: "50127",
        sl_no: "005",
        marks: "090",
        hmarks: "090",
        bdexcode: "01111000010101010101010100010001",
        bhexcode: "01111000010101010101010100010001",
        err_dscr: "Litho Shift.",
        comments: "Bit alignment shift detected"
      },
      {
        id: 106,
        serial: 6,
        pap_code: "B0139H",
        eb_no: "50128",
        sl_no: "006",
        marks: "055",
        hmarks: "055",
        bdexcode: "11110000101010101010101000100010",
        bhexcode: "00000000101010101010101000100010",
        err_dscr: "Litho 1,2 NE",
        comments: "Dexcode and Hexcode mismatch"
      },
      {
        id: 107,
        serial: 7,
        pap_code: "B0139H",
        eb_no: "50129",
        sl_no: "007",
        marks: "105",
        hmarks: "105",
        bdexcode: "11110010010010010010010000100010",
        bhexcode: "11110010010010010010010000100010",
        err_dscr: "Marks. Len.",
        comments: "Check marks range"
      },
      {
        id: 108,
        serial: 8,
        pap_code: "B0139H",
        eb_no: "50130",
        sl_no: "008",
        marks: "088",
        hmarks: "088",
        bdexcode: "11110000111100001111000000100010",
        bhexcode: "11110000111100001111000000100010",
        err_dscr: "",
        comments: "OK"
      }
    ]
  }
};
