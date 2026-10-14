// NU H-Type Solver - Modern Engine & Web Application Logic

class NUSolverApp {
  constructor() {
    this.files = window.NU_SEED_DATA.files;
    this.solvers = window.NU_SEED_DATA.solvers;
    this.validPaperCodes = window.NU_SEED_DATA.validPaperCodes;
    this.currentUser = null;
    this.currentTheme = localStorage.getItem("nu_solver_theme") || "obsidian-emerald";
    this.currentFile = "B0139H1A.txt";
    this.currentRows = JSON.parse(JSON.stringify(window.NU_SEED_DATA.sampleRows["B0139H1A.txt"] || []));
    this.selectedRowId = null;
    this.filterText = "";
    this.errorOnly = false;
    this.modifiedRowsCount = 0;

    this.initUI();
    this.applyTheme(this.currentTheme);
    this.checkSession();
  }

  // Authentication & Session Management
  checkSession() {
    const savedUser = sessionStorage.getItem("nu_solver_user");
    if (savedUser) {
      try {
        this.currentUser = JSON.parse(savedUser);
        this.onUserLoggedIn();
        return;
      } catch (e) {}
    }
    this.showLoginModal();
  }

  showLoginModal() {
    document.getElementById("loginOverlay").classList.remove("hidden");
  }

  hideLoginModal() {
    document.getElementById("loginOverlay").classList.add("hidden");
  }

  handleLogin() {
    const usernameInput = document.getElementById("loginUsername").value.trim().toLowerCase();
    const passwordInput = document.getElementById("loginPassword").value.trim();

    const matchedSolver = this.solvers.find(s => 
      s.username.toLowerCase() === usernameInput && s.passkey === passwordInput
    );

    if (matchedSolver) {
      this.currentUser = matchedSolver;
      sessionStorage.setItem("nu_solver_user", JSON.stringify(matchedSolver));
      document.getElementById("loginError").style.display = "none";
      this.onUserLoggedIn();
    } else {
      document.getElementById("loginError").style.display = "block";
    }
  }

  quickLogin(username, passkey) {
    document.getElementById("loginUsername").value = username;
    document.getElementById("loginPassword").value = passkey;
    this.handleLogin();
  }

  logout() {
    sessionStorage.removeItem("nu_solver_user");
    this.currentUser = null;
    this.showLoginModal();
  }

  onUserLoggedIn() {
    this.hideLoginModal();

    // Update User Badge in Navbar
    const avatar = document.getElementById("userAvatar");
    const badgeText = document.getElementById("userBadgeText");
    avatar.innerText = (this.currentUser.fullName || this.currentUser.username)[0].toUpperCase();
    badgeText.innerHTML = `Operator: <strong>${this.currentUser.fullName || this.currentUser.username}</strong> (${this.currentUser.role})`;

    // Filter or select relevant file for logged in solver
    const assignedFile = this.files.find(f => f.solverName === this.currentUser.username);
    if (assignedFile) {
      this.selectFile(assignedFile.fileName);
    } else {
      this.render();
    }
  }

  // Theme Switcher
  applyTheme(themeName) {
    document.documentElement.setAttribute("data-theme", themeName);
    document.getElementById("themeSelector").value = themeName;
    localStorage.setItem("nu_solver_theme", themeName);
    this.currentTheme = themeName;
  }

  changeTheme(themeName) {
    this.applyTheme(themeName);
  }

  // Business Logic Validation Rules
  validateRow(row) {
    let errors = [];

    // 1. Paper Code validation
    if (!row.pap_code || !/^\d{6}$/.test(row.pap_code) && !this.validPaperCodes.includes(row.pap_code)) {
      errors.push("Sub. Invalid");
    }

    // 2. Examiner Board Code (eb_no) length check
    if (!row.eb_no || !/^\d+$/.test(row.eb_no)) {
      errors.push("EB. IC.");
    } else if (row.eb_no.length !== 5) {
      errors.push("EB. Len.");
    }

    // 3. Serial Number length check
    if (!row.sl_no || !/^\d+$/.test(row.sl_no)) {
      errors.push("SL. IC.");
    } else if (row.sl_no.length !== 3) {
      errors.push("SL. Len.");
    }

    // 4. Marks check
    if (!row.marks || !/^\d+$/.test(row.marks)) {
      errors.push("Marks. IC.");
    } else if (row.marks.length !== 3 || parseInt(row.marks, 10) > 100) {
      errors.push("Marks. Len.");
    }

    // 5. Lithocode Checks
    const lithoErr = this.checkLitho(row.bdexcode, row.bhexcode);
    if (lithoErr) {
      errors.push(lithoErr);
    }

    row.err_dscr = errors.join(" ");
    return row.err_dscr;
  }

  checkLitho(dexcode, hexcode) {
    if (!dexcode || !hexcode) return "Litho Empty";
    if (dexcode.length !== 32 || hexcode.length !== 32) return "Litho Script Len";
    if (dexcode !== hexcode) return "Litho 1,2 NE";

    const header = dexcode.substring(0, 4);
    const footer = dexcode.substring(28, 32);

    const validFraming = [
      { h: "1111", f: "0010" },
      { h: "1111", f: "1000" },
      { h: "1111", f: "0011" },
      { h: "1100", f: "1110" },
      { h: "1100", f: "0001" },
      { h: "0010", f: "1111" },
      { h: "1010", f: "0100" },
      { h: "1010", f: "0101" },
      { h: "1010", f: "0110" },
      { h: "1011", f: "0100" }
    ];

    const isMatched = validFraming.some(rule => rule.h === header && rule.f === footer);
    if (!isMatched) {
      return "Litho Shift.";
    }

    return "";
  }

  // Smart Auto-Fix Heuristic Engine
  autoFixRow(row) {
    let fixed = false;

    if (row.eb_no && /^\d+$/.test(row.eb_no) && row.eb_no.length < 5) {
      row.eb_no = row.eb_no.padStart(5, '0');
      fixed = true;
    }

    if (row.sl_no && /^\d+$/.test(row.sl_no) && row.sl_no.length < 3) {
      row.sl_no = row.sl_no.padStart(3, '0');
      fixed = true;
    }

    if (row.marks && /^\d+$/.test(row.marks) && row.marks.length < 3) {
      row.marks = row.marks.padStart(3, '0');
      fixed = true;
    }

    if (row.err_dscr.includes("Litho Shift.") && row.bdexcode && row.bdexcode.length === 32) {
      let idx = row.bdexcode.indexOf("1111");
      if (idx > 0 && idx < 4) {
        let shifted = row.bdexcode.substring(idx) + "0".repeat(idx);
        if (shifted.length === 32) {
          row.bdexcode = shifted;
          row.bhexcode = shifted;
          fixed = true;
        }
      }
    }

    if (row.err_dscr.includes("Litho 1,2 NE")) {
      row.bhexcode = row.bdexcode;
      fixed = true;
    }

    this.validateRow(row);
    return fixed;
  }

  batchAutoFix() {
    let count = 0;
    this.currentRows.forEach(row => {
      if (row.err_dscr) {
        if (this.autoFixRow(row)) {
          count++;
        }
      }
    });
    this.modifiedRowsCount += count;
    this.render();
    alert(`Smart Auto-Fix applied! Corrected ${count} anomalous rows automatically.`);
  }

  // UI Event Handlers & Rendering
  initUI() {
    document.getElementById("btnAutoFix").addEventListener("click", () => this.batchAutoFix());
    document.getElementById("btnExportText").addEventListener("click", () => this.exportOMRText());
    document.getElementById("btnExportSQL").addEventListener("click", () => this.exportSQLScript());
    document.getElementById("btnMarkSolved").addEventListener("click", () => this.markFileSolved());
    document.getElementById("btnUploadFile").addEventListener("click", () => this.handleFileUploadPrompt());
    document.getElementById("searchInput").addEventListener("input", (e) => {
      this.filterText = e.target.value.toLowerCase();
      this.renderGrid();
    });
    document.getElementById("chkErrorOnly").addEventListener("change", (e) => {
      this.errorOnly = e.target.checked;
      this.renderGrid();
    });

    document.getElementById("modalClose").addEventListener("click", () => {
      document.getElementById("lithoModal").classList.remove("active");
    });
  }

  render() {
    this.renderFileList();
    this.renderStats();
    this.renderGrid();
  }

  renderStats() {
    const totalFiles = this.files.length;
    const solvedFiles = this.files.filter(f => f.status === "SOLVED").length;
    const totalRowsCount = this.currentRows.length;
    const errorRowsCount = this.currentRows.filter(r => r.err_dscr && r.err_dscr.trim() !== "").length;
    const cleanRowsCount = totalRowsCount - errorRowsCount;

    document.getElementById("statTotalFiles").innerText = `${solvedFiles}/${totalFiles}`;
    document.getElementById("statTotalRows").innerText = totalRowsCount;
    document.getElementById("statErrorRows").innerText = errorRowsCount;
    document.getElementById("statCleanCleanRate").innerText = totalRowsCount > 0 
      ? `${Math.round((cleanRowsCount / totalRowsCount) * 100)}%` 
      : "100%";
  }

  renderFileList() {
    const listEl = document.getElementById("fileList");
    listEl.innerHTML = "";

    this.files.forEach(f => {
      const div = document.createElement("div");
      div.className = `file-item ${f.fileName === this.currentFile ? "active" : ""}`;
      div.onclick = () => this.selectFile(f.fileName);

      let badgeClass = "badge-pending";
      if (f.status === "SOLVED") badgeClass = "badge-solved";
      if (f.status === "SOLVING") badgeClass = "badge-solving";

      div.innerHTML = `
        <div>
          <div class="file-name">${f.fileName}</div>
          <div class="file-meta">Paper: ${f.paperCode} • Solver: ${f.solverName || 'Unassigned'}</div>
        </div>
        <span class="badge ${badgeClass}">${f.status}</span>
      `;
      listEl.appendChild(div);
    });
  }

  selectFile(fileName) {
    this.currentFile = fileName;
    if (!window.NU_SEED_DATA.sampleRows[fileName]) {
      window.NU_SEED_DATA.sampleRows[fileName] = this.generateRowsForFile(fileName);
    }
    this.currentRows = JSON.parse(JSON.stringify(window.NU_SEED_DATA.sampleRows[fileName]));
    this.currentRows.forEach(r => this.validateRow(r));
    this.render();
  }

  generateRowsForFile(fileName) {
    const rows = [];
    const paperCode = fileName.substring(0, 6);
    for (let i = 1; i <= 50; i++) {
      const isErr = i % 7 === 0;
      rows.push({
        id: 200 + i,
        serial: i,
        pap_code: paperCode,
        eb_no: isErr ? "501" : "50144",
        sl_no: String(i).padStart(3, '0'),
        marks: String(50 + (i % 40)).padStart(3, '0'),
        hmarks: String(50 + (i % 40)).padStart(3, '0'),
        bdexcode: "11110000101010101010101000100010",
        bhexcode: "11110000101010101010101000100010",
        err_dscr: isErr ? "EB. Len." : "",
        comments: ""
      });
    }
    return rows;
  }

  renderGrid() {
    const tbody = document.getElementById("solverGridBody");
    tbody.innerHTML = "";

    const filteredRows = this.currentRows.filter(r => {
      if (this.errorOnly && (!r.err_dscr || r.err_dscr.trim() === "")) return false;
      if (this.filterText) {
        const text = `${r.serial} ${r.pap_code} ${r.eb_no} ${r.sl_no} ${r.err_dscr} ${r.comments}`.toLowerCase();
        return text.includes(this.filterText);
      }
      return true;
    });

    filteredRows.forEach(row => {
      const tr = document.createElement("tr");
      const hasError = row.err_dscr && row.err_dscr.trim() !== "";
      if (hasError) tr.classList.add("has-error");

      tr.innerHTML = `
        <td style="font-family: 'JetBrains Mono', monospace; font-weight:700;">#${row.serial}</td>
        <td>
          ${hasError 
            ? `<span class="badge badge-error">${row.err_dscr}</span>` 
            : `<span class="badge badge-solved">OK</span>`}
        </td>
        <td>
          <input type="text" class="grid-input" value="${row.pap_code}" maxlength="6" 
            onchange="window.app.updateRowCell(${row.id}, 'pap_code', this.value)" />
        </td>
        <td>
          <input type="text" class="grid-input" value="${row.eb_no}" maxlength="5" 
            onchange="window.app.updateRowCell(${row.id}, 'eb_no', this.value)" />
        </td>
        <td>
          <input type="text" class="grid-input" value="${row.sl_no}" maxlength="3" 
            onchange="window.app.updateRowCell(${row.id}, 'sl_no', this.value)" />
        </td>
        <td>
          <input type="text" class="grid-input" value="${row.marks}" maxlength="3" 
            onchange="window.app.updateRowCell(${row.id}, 'marks', this.value)" />
        </td>
        <td>
          <div class="binary-code" onclick="window.app.openLithoInspector(${row.id})">
            ${row.bdexcode.substring(0, 16)}...
          </div>
        </td>
        <td>
          <button class="btn btn-outline" style="padding: 0.25rem 0.6rem; font-size: 0.75rem;" 
            onclick="window.app.openLithoInspector(${row.id})">
            🔍 Inspect
          </button>
        </td>
      `;
      tbody.appendChild(tr);
    });
  }

  updateRowCell(id, field, value) {
    const row = this.currentRows.find(r => r.id === id);
    if (row) {
      row[field] = value;
      this.validateRow(row);
      this.modifiedRowsCount++;
      this.render();
    }
  }

  openLithoInspector(rowId) {
    this.selectedRowId = rowId;
    const row = this.currentRows.find(r => r.id === rowId);
    if (!row) return;

    document.getElementById("modalRowInfo").innerText = `Row #${row.serial} - File: ${this.currentFile}`;
    this.renderBitChips("dexcodeBits", row.bdexcode, (newVal) => {
      row.bdexcode = newVal;
      this.validateRow(row);
      this.render();
      this.openLithoInspector(rowId);
    });

    this.renderBitChips("hexcodeBits", row.bhexcode, (newVal) => {
      row.bhexcode = newVal;
      this.validateRow(row);
      this.render();
      this.openLithoInspector(rowId);
    });

    document.getElementById("lithoModalStatus").innerHTML = row.err_dscr 
      ? `<span class="badge badge-error">${row.err_dscr}</span>` 
      : `<span class="badge badge-solved">Valid Framing OK</span>`;

    document.getElementById("lithoModal").classList.add("active");
  }

  renderBitChips(containerId, bitString, onBitToggle) {
    const container = document.getElementById(containerId);
    container.innerHTML = "";

    for (let i = 0; i < 32; i++) {
      const bitVal = bitString[i] || '0';
      const chip = document.createElement("div");
      let classes = `bit-chip bit-${bitVal}`;

      if (i < 4) classes += " header-bit";
      if (i >= 28) classes += " footer-bit";

      chip.className = classes;
      chip.innerText = bitVal;
      chip.title = `Bit ${i} (${i < 4 ? 'Header' : i >= 28 ? 'Footer' : 'Payload'})`;
      chip.onclick = () => {
        const toggled = bitVal === '1' ? '0' : '1';
        const newBitString = bitString.substring(0, i) + toggled + bitString.substring(i + 1);
        onBitToggle(newBitString);
      };
      container.appendChild(chip);
    }
  }

  markFileSolved() {
    const unresolvedErrors = this.currentRows.filter(r => r.err_dscr && r.err_dscr.trim() !== "").length;
    if (unresolvedErrors > 0) {
      if (!confirm(`Warning: This file still contains ${unresolvedErrors} unresolved errors. Mark as SOLVED anyway?`)) {
        return;
      }
    }

    const file = this.files.find(f => f.fileName === this.currentFile);
    if (file) {
      file.status = "SOLVED";
      file.correctedRows = this.currentRows.length - unresolvedErrors;
      file.endDate = new Date().toISOString().split('T')[0];
      this.render();
      alert(`File '${this.currentFile}' successfully marked as SOLVED!`);
    }
  }

  exportOMRText() {
    let output = "";
    this.currentRows.forEach(r => {
      output += `${String(r.serial).padStart(10, '0')}...${r.pap_code}${r.eb_no}${r.sl_no}${r.marks}${r.hmarks}${r.bdexcode}\n`;
    });

    const blob = new Blob([output], { type: "text/plain" });
    const url = URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = `SOLVED_${this.currentFile}`;
    a.click();
  }

  exportSQLScript() {
    let sql = `-- NU H-Type Solver - Automated SQL Update Script for ${this.currentFile}\n`;
    this.currentRows.forEach(r => {
      sql += `UPDATE [dbo].[H_solving] SET pap_code='${r.pap_code}', eb_no='${r.eb_no}', sl_no='${r.sl_no}', marks='${r.marks}', bdexcode='${r.bdexcode}', bhexcode='${r.bhexcode}', err_dscr='${r.err_dscr}', last_update=GETDATE() WHERE id=${r.id};\n`;
    });

    const blob = new Blob([sql], { type: "text/sql" });
    const url = URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = `UPDATE_${this.currentFile}.sql`;
    a.click();
  }

  handleFileUploadPrompt() {
    const fileName = prompt("Enter new OMR Scan Text File Name (e.g. B0149H1A.txt):", "B0149H1A.txt");
    if (fileName) {
      const solverUsername = this.currentUser ? this.currentUser.username : "rakib";
      const newFile = {
        fileName: fileName,
        paperCode: fileName.substring(0, 6),
        solverName: solverUsername,
        status: "SOLVING",
        totalRows: 60,
        correctedRows: 0,
        startDate: new Date().toISOString().split('T')[0],
        endDate: null
      };
      this.files.push(newFile);
      this.selectFile(fileName);
    }
  }
}

// Global initialization
window.addEventListener("DOMContentLoaded", () => {
  window.app = new NUSolverApp();
});
