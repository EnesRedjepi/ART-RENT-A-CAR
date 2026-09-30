import { useMemo, useState } from "react";
import type { ReactNode } from "react";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline";
import { ThemeContext } from "./theme-context";

export const CustomThemeProvider = ({ children }: { children: ReactNode }) => {
  const [mode, setMode] = useState<"light" | "dark">(() => {
    const saved = localStorage.getItem("themeMode") as "light" | "dark" | null;
    return saved === "light" ? "light" : "dark";
  });

  const toggleTheme = () => {
    const newMode = mode === "dark" ? "light" : "dark";
    setMode(newMode);
    localStorage.setItem("themeMode", newMode);
  };

  const theme = useMemo(
    () =>
      createTheme({
        palette: {
          mode,
          primary: {
            main: mode === "dark" ? "#65B99B" : "#176B56",
          },
          secondary: {
            main: mode === "dark" ? "#E4B35B" : "#A96F18",
          },
          background: {
            default: mode === "dark" ? "#111816" : "#F3F5F2",
            paper: mode === "dark" ? "#1B2420" : "#FFFFFF",
          },
        },
        shape: {
          borderRadius: 8,
        },
        typography: {
          fontFamily: "Poppins, sans-serif",
          h1: { fontWeight: 700, lineHeight: 1.08 },
          h2: { fontWeight: 700, lineHeight: 1.15 },
          h3: { fontWeight: 700, lineHeight: 1.15 },
          button: { fontWeight: 600, textTransform: "none" },
        },
        components: {
          MuiCard: {
            styleOverrides: {
              root: {
                borderRadius: 8,
                border: `1px solid ${mode === "dark" ? "rgba(255,255,255,0.08)" : "rgba(20,40,32,0.1)"}`,
                boxShadow: "none",
              },
            },
          },
          MuiButton: {
            styleOverrides: {
              root: {
                borderRadius: 6,
                minHeight: 44,
              },
            },
          },
        },
      }),
    [mode],
  );

  return (
    <ThemeContext.Provider value={{ toggleTheme, mode }}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        {children}
      </ThemeProvider>
    </ThemeContext.Provider>
  );
};
