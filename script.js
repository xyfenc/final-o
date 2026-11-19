// Built by xyfenc | github.com/xyfenc
const navButtons = document.querySelectorAll(".nav-button[data-page]");
const panels = document.querySelectorAll("[data-page-panel]");
const main = document.querySelector(".main-content");
const textBorder = document.querySelector(".text-border");
const orgChartDialog = document.getElementById("org-chart-dialog");
const ids = [...navButtons].map((b) => b.dataset.page);

function updateTextBorder() {
    const names = "DELCANO=CUMMINS=NOORA=BALMACEDA=";
    const context = document.createElement("canvas").getContext("2d");
    context.font = getComputedStyle(textBorder).font;
    const textWidth = context.measureText(names).width || 30;
    textBorder.textContent = names.repeat(Math.ceil(window.innerWidth / textWidth) + 2);
}

function showPage(id, updateHash = true) {
    const target = document.getElementById(id);
    if (!target) return;
    const index = ids.indexOf(id);
    panels.forEach((p) => { p.hidden = p !== target; });
    navButtons.forEach((b, i) => {
        const active = i === index;
        b.classList.toggle("active", active);
        active ? b.setAttribute("aria-current", "page") : b.removeAttribute("aria-current");
    });
    main.scrollTop = 0;
    if (updateHash && location.hash !== "#" + id) history.pushState(null, "", "#" + id);
}

navButtons.forEach((b) => b.addEventListener("click", () => showPage(b.dataset.page)));
document.querySelectorAll("[data-open-org-chart]").forEach((b) => b.addEventListener("click", () => orgChartDialog.showModal()));
document.querySelector("[data-close-org-chart]").addEventListener("click", () => orgChartDialog.close());
orgChartDialog.addEventListener("click", (event) => {
    if (event.target === orgChartDialog) orgChartDialog.close();
});
document.querySelectorAll("[data-back]").forEach((b) => b.addEventListener("click", () => showPage("home")));
document.querySelectorAll("[data-page-link]").forEach((b) => b.addEventListener("click", () => showPage(b.dataset.pageLink)));
window.addEventListener("popstate", () => showPage(location.hash.slice(1) || "home", false));
showPage(location.hash.slice(1) || "home", false);
updateTextBorder();
window.addEventListener("resize", updateTextBorder);

document.querySelectorAll(".brand-mark img").forEach((img) => {
    const hide = () => { if (!img.naturalWidth) img.hidden = true; };
    img.addEventListener("error", hide);
    if (img.complete) hide();
});

const themeToggle = document.getElementById("theme-toggle");
function setTheme(theme) {
    document.documentElement.dataset.theme = theme;
    themeToggle.textContent = theme === "dark" ? "Light mode" : "Dark mode";
    themeToggle.setAttribute("aria-pressed", theme === "dark");
    try { localStorage.setItem("bdcn-theme", theme); } catch (e) {}
}
let savedTheme = "light";
try { savedTheme = localStorage.getItem("bdcn-theme") || "light"; } catch (e) {}
setTheme(savedTheme);
themeToggle.addEventListener("click", () => setTheme(document.documentElement.dataset.theme === "dark" ? "light" : "dark"));