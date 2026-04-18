"use client";

import { useState } from "react";
import EntitiesSection from "@/components/EntitiesSection";
import DataSection from "@/components/DataSection";
import type { EntityType } from "@/types/entity";

export default function EntityExplorer() {
  const [selectedEntity, setSelectedEntity] = useState<EntityType>("ages");

  return (
    <>
      <EntitiesSection
        selectedEntity={selectedEntity}
        onSelectEntity={setSelectedEntity}
      />
      <DataSection
        selectedEntity={selectedEntity}
        onSelectEntity={setSelectedEntity}
      />
    </>
  );
}
