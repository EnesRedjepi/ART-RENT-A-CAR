import Navbar from "./layout/Navbar";

const Layout = ({ children }: { children: React.ReactNode }) => {
  return (
    <>
      <Navbar />
      <main style={{ paddingTop: "80px" }}>{children}</main>
    </>
  );
};

export default Layout;
