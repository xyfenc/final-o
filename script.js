// Built by xyfenc | github.com/xyfenc
const navButtons = document.querySelectorAll(".nav-button");
const panels = document.querySelectorAll("[data-page-panel]");
const main = document.querySelector(".main-content");
const textBorder = document.querySelector(".text-border");
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
// interactive organizational chart
const orgPopup = document.createElement("div");
orgPopup.className = "org-popup";
orgPopup.innerHTML = `
<div class="org-popup-content" role="dialog"
aria-modal="true"
aria-labelledby="org-popup-title">
<button class="org-popup-close" aria-label="Close">
&times;
</button>
<span class="org-popup-position"></span>
<h3 id="org-popup-title"></h3>
<p class="org-popup-description"></p>
<h4>Responsibilities</h4>
<ul class="org-popup-responsibilities"></ul>
</div>
`;
document.body.appendChild(orgPopup);
const popupTitle = orgPopup.querySelector("h3");
const popupPosition = orgPopup.querySelector(".org-popup-position");
const popupDescription = orgPopup.querySelector(".org-popup-description");
const popupResponsibilities = orgPopup.querySelector(".org-popup-responsibilities");
const popupClose = orgPopup.querySelector(".org-popup-close");
function openOrgPopup(node) {
const name = node.querySelector("strong")?.textContent.trim() || "Member";
const position = node.dataset.position || "Position not specified";
const description = node.dataset.description || "No description available.";
const responsibilities = node.dataset.responsibilities || "Not specified.";
popupTitle.textContent = name;
popupPosition.textContent = position;
popupDescription.textContent = description;
popupResponsibilities.replaceChildren();
responsibilities.split(";").forEach((item) => {
const li = document.createElement("li");
li.textContent = item.trim();
popupResponsibilities.appendChild(li);
});
orgPopup.classList.add("active");
popupClose.focus();
}
function closeOrgPopup() {
orgPopup.classList.remove("active");
}
document.querySelectorAll(".org-chart .org-node, .org-chart .staff-node").forEach((node) => {
node.addEventListener("click", () => openOrgPopup(node));
node.addEventListener("keydown", (e) => {
if (e.key === "Enter" || e.key === " ") {
e.preventDefault();
openOrgPopup(node);
}
});
});
popupClose.addEventListener("click", closeOrgPopup);
orgPopup.addEventListener("click", (e) => {
if (e.target === orgPopup) {
closeOrgPopup();
}
});
document.addEventListener("keydown", (e) => {
if (e.key === "Escape") {
closeOrgPopup();
}
});
/* WEBSITE LOADING SCREEN */
window.addEventListener("load", () => {
const loader = document.getElementById("page-loader");
if (loader) {
setTimeout(() => {
loader.classList.add("hidden");
setTimeout(() => {
loader.remove();
}, 500);
}, 500);
}
});
