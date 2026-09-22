export async function GetNewGame() {
  const response = await fetch("/get/savefiles/create");
  if (!response.ok) {
    throw new Error("Failed to create new game save file");
  }
  return true;
}
