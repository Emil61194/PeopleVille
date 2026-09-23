const enumLabels = {
  0: "Cashier",
  1: "Sales representative",
  2: "Marketing specialist",
};

const genderLabels = {
  0: "Man",
  1: "Woman",
  2: "Neutral",
};

export function getEnumLabel(value) {
  const rawValue =
    value && typeof value === "object" ? (value.value ?? value.Value) : value;
  const numericValue = Number(rawValue);

  if (Number.isInteger(numericValue) && `${rawValue}`.match(/^\d+$/)) {
    return enumLabels[numericValue] ?? String(rawValue);
  }

  return rawValue
    ? String(rawValue)
        .replace(/([a-z])([A-Z])/g, "$1 $2")
        .replace(/^./, (letter) => letter.toUpperCase())
    : "-";
}

export function getGenderLabel(value) {
  const rawValue =
    value && typeof value === "object" ? (value.value ?? value.Value) : value;
  const numericValue = Number(rawValue);

  if (Number.isInteger(numericValue) && `${rawValue}`.match(/^\d+$/)) {
    return genderLabels[numericValue] ?? String(rawValue);
  }

  return rawValue
    ? String(rawValue)
        .replace(/([a-z])([A-Z])/g, "$1 $2")
        .replace(/^./, (letter) => letter.toUpperCase())
    : "-";
}

const fieldLabels = {
  id: "ID",
  firstName: "First name",
  lastName: "Last name",
  birth: "Birth date",
  gender: "Gender",
  homeAddress: "Home address",
  currentLocation: "Current location",
  job: "Job",
  school: "School",
  address: "Address",
  rent: "Rent",
  floors: "Floors",
  citizenCapacity: "Citizen capacity",
  foodInventory: "Food inventory",
  waterInventory: "Water inventory",
  bankAccount: "Bank account",
  balance: "Balance",
  transactions: "Transactions",
  jobTitle: "Job title",
  jobCapacity: "Job capacity",
  salary: "Salary",
  workStartTime: "Work start",
  workEndTime: "Work end",
  foodPrice: "Food price",
  waterPrice: "Water price",
  workplace: "Workplace",
};

export function getFieldLabel(key) {
  return (
    fieldLabels[key] ??
    key
      .replace(/[A-Z]/g, (letter) => ` ${letter}`)
      .replace(/^./, (letter) => letter.toUpperCase())
  );
}

export function getEntityFields(type, item) {
  if (!item) return [];

  if (type === "citizen") {
    return [
      ["id", item.id],
      ["firstName", item.firstName],
      ["lastName", item.lastName],
      ["birth", item.birth],
      ["gender", getGenderLabel(item.gender)],
      ["homeAddress", item.homeAddress],
      ["currentLocation", item.currentLocation],
      ["job", item.job?.workplace],
      ["school", item.school],
    ];
  }

  if (type === "home") {
    return [
      ["address", item.address],
      ["rent", item.rent],
      ["floors", item.floors],
      ["citizenCapacity", item.citizenCapacity],
      ["foodInventory", item.foodInventory],
      ["waterInventory", item.waterInventory],
      ["bankAccount", item.bankAccount],
    ];
  }

  return [
    ["address", item.address],
    ["jobTitle", getEnumLabel(item.jobTitle)],
    ["jobCapacity", item.jobCapacity],
    ["salary", item.salary],
    ["workStartTime", item.workStartTime],
    ["workEndTime", item.workEndTime],
    ["foodPrice", item.foodPrice],
    ["waterPrice", item.waterPrice],
  ];
}
